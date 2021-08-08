namespace CodeforcesApi.Core.Models
{
    public enum ParticipantType { CONTESTANT, PRACTICE, VIRTUAL, MANAGER, OUT_OF_COMPETITION }
    /// <summary>
    /// Represents a party, participating in a contest.
    /// </summary>
    public class PartyDto
    {
        /// <summary>
        /// Id of the contest, in which party is participating.
        /// </summary>
        /// <reamarks>
        /// Can be absent.
        /// </remarks>
        public int? ContestId { get; set; }
        public ParticipantType ParticipantType { get; set; }
        /// <summary>
        /// Members of the party.
        /// </summary>
        public MemberDto[] Members { get; set; }
        /// <summary>
        /// If party is a team, then it is a unique team id. Otherwise, this field is absent.
        /// </summary>
        /// <reamarks>
        /// Can be absent.
        /// </remarks>
        public int? TeamId { get; set; }
        /// <summary>
        /// If party is a team or ghost, then it is a localized name of the team. Otherwise, it is absent.
        /// Localized.
        /// </summary>
        /// <reamarks>
        /// Can be absent.
        /// </remarks>
        public string TeamName { get; set; }
        /// <summary>
        /// If <see cref="true"/> then this party is a ghost. It participated in the contest, but not on Codeforces. For example, Andrew Stankevich Contests in Gym has ghosts of the participants from Petrozavodsk Training Camp.
        /// </summary>
        public bool Ghost { get; set; }
        /// <summary>
        /// Room of the party. If absent, then the party has no room.
        /// </summary>
        /// <reamarks>
        /// Can be absent.
        /// </remarks>
        public int? Room { get; set; }
        /// <summary>
        /// Time, when this party started a contest.
        /// </summary>
        /// <reamarks>
        /// Can be absent.
        /// </remarks>
        public int? StartTimeSeconds { get; set; }
    }
}