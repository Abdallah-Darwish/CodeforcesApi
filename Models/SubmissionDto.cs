using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesApi.Core.Models
{
    public enum SubmissionVerdict { FAILED, OK, PARTIAL, COMPILATION_ERROR, RUNTIME_ERROR, WRONG_ANSWER, PRESENTATION_ERROR, TIME_LIMIT_EXCEEDED, MEMORY_LIMIT_EXCEEDED, IDLENESS_LIMIT_EXCEEDED, SECURITY_VIOLATED, CRASHED, INPUT_PREPARATION_CRASHED, CHALLENGED, SKIPPED, TESTING, REJECTED }
    public enum SubmissionTestSet { SAMPLES, PRETESTS, TESTS, CHALLENGES, TESTS1, TESTS2, TESTS3, TESTS4, TESTS5, TESTS6, TESTS7, TESTS8, TESTS9, TESTS10 }
    /// <summary>
    /// Represents a submission.
    /// </summary>
    public class SubmissionDto : IEquatable<SubmissionDto>
    {
        public int Id { get; set; }
        /// <summary>
        /// Can be absent.
        /// </summary>
        public int? ContestId { get; set; }
        /// <summary>
        /// Time, when submission was created, in unix-format.
        /// </summary>
        public int CreationTimeSeconds { get; set; }
        /// <summary>
        /// Number of seconds, passed after the start of the contest (or a virtual start for virtual parties), before the submission.
        /// </summary>
        public int RelativeTimeSeconds { get; set; }
        public ProblemDto Problem { get; set; }
        public PartyDto Author { get; set; }
        public string ProgrammingLanguage { get; set; }
        /// <summary>
        /// Can be absent.
        /// </summary>
        public SubmissionVerdict? Verdict { get; set; }
        /// <summary>
        /// Testset used for judging the submission.
        /// </summary>
        public SubmissionTestSet TestSet { get; set; }
        /// <summary>
        /// Number of passed tests.
        /// </summary>
        public int PassedTestCount { get; set; }
        /// <summary>
        /// Maximum time in milliseconds, consumed by solution for one test.
        /// </summary>
        public int TimeConsumedMillis { get; set; }
        /// <summary>
        /// Maximum memory in bytes, consumed by solution for one test.
        /// </summary>
        public int MemoryConsumedBytes { get; set; }

        public bool Equals(SubmissionDto other)
        {
            if (other == null) { return false; }
            return Id == other.Id;
        }
        public override int GetHashCode() => Id;
        public override bool Equals(object obj)
        {
            if (obj is SubmissionDto sub) { return Equals(sub); }
            return false;
        }
    }
}
