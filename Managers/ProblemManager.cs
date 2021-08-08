using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using CodeforcesApi.Core.Entities;
using CodeforcesApi.Core.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CodeforcesApi.Core.Managers
{
    internal class ProblemManager
    {
        public async ValueTask Insert(ProblemDto problem)
        {
            using var con = DatabaseManager.GetConnection();
            if ((await con.ExecuteScalarAsync<string?>("SELECT id FROM Problem WHERE id = @problemId", new { problemId = problem.Id }).ConfigureAwait(false)) == null)
            {
                await con.ExecuteAsync("INSERT INTO Problem(Id, ContestId, [Index], Name) VALUES(@problemId, @contestId, @index, @name)", new { problemId = problem.Id, problem.ContestId, problem.Index, problem.Name }).ConfigureAwait(false);
                await con.ExecuteAsync("INSERT INTO ProblemInfo(ProblemId, Name, [Index], Rating) VALUES(@problemId, @name, @index, @rating)", new { problemId = problem.Id, problem.ContestId, problem.Name, problem.Index, problem.Rating }).ConfigureAwait(false);
            }
        }
        public Problem GetProblem(string problemId)
        {
            using var con = DatabaseManager.GetConnection();
            return con.QueryFirst<Problem>("SELECT * FROM Problem WHERE id = @problemId", new { problemId });
        }
        public async ValueTask<Problem> GetProblemAsync(string problemId)
        {
            using var con = DatabaseManager.GetConnection();
            return await con.QueryFirstOrDefaultAsync<Problem>("SELECT * FROM Problem WHERE id = @problemId", new { problemId }).ConfigureAwait(false);
        }
        public async ValueTask<IEnumerable<Problem>> GetProblems(IEnumerable<string> problemsIds)
        {
            using var con = DatabaseManager.GetConnection();
            return await con.QueryAsync<Problem>("SELECT * FROM Problem WHERE id IN @problemsIds", new { problemsIds }).ConfigureAwait(false);
        }

        public async ValueTask<IEnumerable<Problem>> GetProblemsByContest(int contestId)
        {
            using var con = DatabaseManager.GetConnection();
            return await con.QueryAsync<Problem>("SELECT * FROM Problem WHERE contestId = @contestId ORDER BY id", new { contestId }).ConfigureAwait(false);
        }

        public async ValueTask<ProblemInfo?> GetProblemInfo(string problemId, CancellationToken cancellationToken = default)
        {

            if (cancellationToken.IsCancellationRequested) { return null; }

            using var con = DatabaseManager.GetConnection();
            ProblemInfo result = await con.QueryFirstOrDefaultAsync<ProblemInfo>("SELECT * FROM ProblemInfo WHERE problemId = @problemId", new { problemId }).ConfigureAwait(false);
            if (result != null) { return result; }

            if (cancellationToken.IsCancellationRequested) { return null; }


            using var questionDocument = await CodeforcesClient.Instance.GetProblemPage(Problem.GetProblemUri(problemId), cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested || questionDocument == null) { return null; }

            var ratingSpan = questionDocument.QuerySelector<IHtmlSpanElement>("span.tag-box[title=\"Difficulty\"]");

            var titleDivContent = questionDocument.QuerySelector<IHtmlDivElement>("div.title").TextContent;
            int indexTitleSeperatorIndex = titleDivContent.IndexOf('.');

            result = new ProblemInfo
            {
                ProblemId = problemId,
                Index = titleDivContent.Substring(0, indexTitleSeperatorIndex).Trim(),
                Name = titleDivContent.Substring(indexTitleSeperatorIndex + 1).Trim(),
                Rating = ratingSpan != null ? (int?)int.Parse(ratingSpan.TextContent.Replace("*", "")) : null
            };
            await con.ExecuteAsync("INSERT INTO ProblemInfo(problemId, [index], [name], rating) VALUES(@problemId, @index, @name, @rating)", result).ConfigureAwait(false);
            return result;

        }
    }
}
