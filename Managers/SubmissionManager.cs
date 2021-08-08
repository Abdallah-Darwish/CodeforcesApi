using CodeforcesApi.Core.Entities;
using CodeforcesApi.Core.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CodeforcesApi.Core.Managers
{
    public class SubmissionManager
    {
        const int UserSubmissionsBulkSize = 30;
        private readonly ProblemManager _problemManager = new ProblemManager();
        public SubmissionManager()
        {
            _problemManager = new ProblemManager();
        }
        public async ValueTask RefreshUserSubmissions(string userHandle)
        {
            using var con = DatabaseManager.GetConnection();
            userHandle = userHandle.ToLowerInvariant();
            int bulkSize = UserSubmissionsBulkSize;
            CodeforcesResponse<SubmissionDto[]> submissionsBulkResponse;
            int? lastSavedSubmissionId = await con.ExecuteScalarAsync<int?>("SELECT id FROM Submission WHERE authorHandle = @userHandle ORDER BY id DESC LIMIT 1", new { userHandle }).ConfigureAwait(false);
            if (lastSavedSubmissionId == null) { bulkSize = 10000; }
            int currentOffset = 1;
            while (true)
            {
                submissionsBulkResponse = await CodeforcesClient.Instance.GetUserSubmissions(userHandle, currentOffset, bulkSize).ConfigureAwait(false);
                //TODO: logg me and dont fail silently
                if (submissionsBulkResponse.IsOk == false) { break; }
                if (submissionsBulkResponse.Result.Length == 0) { break; }
                foreach (var submissionDto in submissionsBulkResponse.Result)
                {
                    if (submissionDto.Id == lastSavedSubmissionId) { goto doneRefreshing; }
                    await _problemManager.Insert(submissionDto.Problem);
                    await con.ExecuteAsync("INSERT OR IGNORE INTO Submission(Id, AuthorHandle, Verdict, ProblemId) VALUES(@id, @userHandle, @verdict, @problemId)", new { submissionDto.Id, userHandle, submissionDto.Verdict, problemId = submissionDto.Problem.Id }).ConfigureAwait(false);
                }

                currentOffset += bulkSize;
            }
        doneRefreshing:
            GC.Collect();
        }

        public async ValueTask<IEnumerable<Submission>> GetUserSubmissionsInContest(string userHandle, int contestId)
        {
            using var con = DatabaseManager.GetConnection();
            return await con.QueryAsync<Submission>(
@"SELECT su.* FROM Submission su
INNER JOIN Problem pr
ON su.problemId = pr.Id
WHERE su.authorHandle = @userHandle AND pr.contestId = @contestId", new { userHandle, contestId }).ConfigureAwait(false);
        }
    }
}
