using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace CodeforcesApi.Core.Models
{
    public enum ProblemType { PROGRAMMING, QUESTION }
    /// <summary>
    /// Represents a problem.
    /// </summary>
    public class ProblemDto : IEquatable<ProblemDto>
    {
        [JsonIgnore]
        public string Id => Entities.Problem.GetProblemId(ContestId, Index);
        /// <summary>
        /// Id of the contest, containing the problem.
        /// Can be absent.
        /// </summary>
        public int? ContestId { get; set; }
        /// <summary>
        /// Short name of the problemset the problem belongs to.
        /// Can be absent.
        /// </summary>
        public string ProblemsetName { get; set; }
        /// <summary>
        /// Usually a letter of a letter, followed by a digit, that represent a problem index in a contest.
        /// </summary>
        public string Index { get; set; }
        /// <summary>
        /// Localized.
        /// </summary>
        public string Name { get; set; }
        public ProblemType Type { get; set; }
        /// <summary>
        /// Maximum ammount of points for the problem.
        /// Can be absent.
        /// </summary>
        public double? Points { get; set; }
        /// <summary>
        /// Problem rating (difficulty).
        /// Can be absent.
        /// </summary>
        public int? Rating { get; set; }
        /// <summary>
        /// Problem tags.
        /// </summary>
        public string[] Tags { get; set; }

        public override string ToString() => Name;
        public override int GetHashCode() => Id.GetHashCode();

        public bool Equals(ProblemDto other)
        {
            if (other == null) { return false; }
            return ContestId == other.ContestId
                && (ProblemsetName?.Equals(other.ProblemsetName, StringComparison.OrdinalIgnoreCase) ?? other.ProblemsetName == null)
                && Index.Equals(other.Index, StringComparison.OrdinalIgnoreCase)
                && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase)
                && Type == other.Type
                && Points == other.Points
                && Rating == other.Rating
                && Tags.SequenceEqual(other.Tags);
        }
        public override bool Equals(object obj)
        {
            if (obj is ProblemDto dto) { return Equals(dto); }
            return false;
        }
    }
}
