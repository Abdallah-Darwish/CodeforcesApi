using CodeforcesApi.Core.Entities;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CodeforcesApi.Core.Managers
{
    public class ContestManager
    {
        public async ValueTask RefreshContests()
        {
            using var con = DatabaseManager.GetConnection();
            var codeforcesResponse = await CodeforcesClient.Instance.GetContests(false).ConfigureAwait(false);
            if (codeforcesResponse.IsOk == false) { return; }
            foreach (var contestModel in codeforcesResponse.Result)
            {
                if ((await con.ExecuteScalarAsync<int?>("SELECT id FROM Contest WHERE id = @contestId", new { contestId = contestModel.Id }).ConfigureAwait(false)) != null) { return; }
                await con.ExecuteAsync("INSERT INTO Contest(Id, Name) VALUES(@id, @name)", new { contestModel.Id, contestModel.Name }).ConfigureAwait(false);
            }
        }
        public async ValueTask<IEnumerable<Contest>> GetEducationalContests()
        {
            using var con = DatabaseManager.GetConnection();
            return await con.QueryAsync<Contest>("SELECT * FROM Contest WHERE name LIKE 'Educational%'").ConfigureAwait(false);
        }
        public async ValueTask<IEnumerable<Contest>> GetAll()
        {
            using var con = DatabaseManager.GetConnection();
            return await con.QueryAsync<Contest>("SELECT * FROM Contest").ConfigureAwait(false);
        }
    }
}
