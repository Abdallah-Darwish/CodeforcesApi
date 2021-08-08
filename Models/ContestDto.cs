using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesApi.Core.Models
{
    public enum ContestType { CF, IOI, ICPC }
    public enum ContestPhase { BEFORE, CODING, PENDING_SYSTEM_TEST, SYSTEM_TEST, FINISHED }
    /// <summary>
    /// Represents a contest on Codeforces.
    /// </summary>
    public class ContestDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Localized.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Scoring system used for the contest.
        /// </summary>
        public ContestType Type { get; set; }
        public ContestPhase Phase { get; set; }
        /// <summary>
        /// If <see cref="true"/>, then the ranklist for the contest is frozen and shows only submissions, created before freeze.
        /// </summary>
        public bool Frozen { get; set; }
        /// <summary>
        /// Duration of the contest in seconds.
        /// </summary>
        public int DurationSeconds { get; set; }
        /// <summary>
        /// Contest start time in unix format.
        /// </summary>
        public int? StartTimeSeconds { get; set; }
        /// <summary>
        /// Number of seconds, passed after the start of the contest.
        /// </summary>
        ///<remarks>
        /// Can be absent.
        /// Can be negative.
        ///</remarks>
        public int? RelativeTimeSeconds { get; set; }
        /// <summary>
        /// Handle of the user, who created the contest.
        /// </summary>
        public string PreparedBy { get; set; }
        /// <summary>
        /// URL for contest-related website.
        /// </summary>
        /// <remarks>
        /// Can be absent.
        /// </remarks>
        public string WebsiteUrl { get; set; }
        /// <summary>
        /// Localized.
        /// </summary>
        /// <reamarks>
        /// Can be absent.
        /// </remarks>
        public string Description { get; set; }
        /// <summary>
        /// From 1 to 5. Larger number means more difficult problems.
        /// </summary>
        /// <remarks>
        /// Can be absent.
        /// </remarks>
        public int? Difficulty { get; set; }
        /// <summary>
        /// Human-readable type of the contest from the following categories: Official ACM-ICPC Contest, Official School Contest, Opencup Contest, School/University/City/Region Championship, Training Camp Contest, Official International Personal Contest, Training Contest.
        /// Localized.
        /// </summary>
        /// <remarks>
        /// Can be absent.
        /// </remarks>
        public string Kind { get; set; }
        /// <summary>
        /// Name of the ICPC Region for official ACM-ICPC contests.
        /// Localized.
        /// </summary>
        /// <remarks>
        /// Can be absent.
        /// </remarks>
        public string ICPCRegion { get; set; }
        /// <summary>
        /// Localized.
        /// </summary>
        /// <remarks>
        /// Can be absent.
        /// </remarks>
        public string Country { get; set; }
        /// <summary>
        /// Localized.
        /// </summary>
        /// <reamarks>
        /// Can be absent.
        /// </remarks>
        public string? City { get; set; }
        /// <remarks>
        /// Can be absent.
        /// </remarks>
        public string? Season { get; set; }
        public override string ToString() => Name;
        public override int GetHashCode() => Id;
    }
}
