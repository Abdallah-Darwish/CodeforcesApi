using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace CodeforcesApi.Core.Entities
{
    [Table("Problem")]
    public class Problem : IEquatable<Problem>
    {
        [ExplicitKey]
        public string Id { get; set; }

        public int? ContestId { get; set; }

        public string Name { get; set; }

        public string Index { get; set; }

        public bool Equals(Problem? other)
        {
            if (other == null) { return false; }
            return Id == other.Id &&
                 ContestId == other.ContestId &&
                 (Name?.Equals(other.Name, StringComparison.OrdinalIgnoreCase) ?? other.Name == null) &&
                 (Index?.Equals(other.Index, StringComparison.OrdinalIgnoreCase) ?? other.Index == null);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Problem problem) { return Equals(problem); }
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Name;

        public static Uri GetProblemUri(string problemId)
        {
            if (problemId.Contains("UNKNOWN", StringComparison.Ordinal)) { throw new ArgumentException("Contest id is unknown.", nameof(problemId)); }
            return new Uri($@"https://www.codeforces.com/{problemId}");
        }

        public static string GetProblemId(int? contestId, string problemIndex) => $@"contest/{(contestId == null ? $"UNKNOW{contestId ?? DateTime.UtcNow.Ticks}{problemIndex}" : contestId.ToString())}/problem/{problemIndex}";
    }
}
