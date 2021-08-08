using System;

namespace CodeforcesApi.Core.Models
{
    /// <summary>
    /// Represents a member of a <see cref="PartyDto"/>.
    /// </summary>
    public class MemberDto : IEquatable<MemberDto>
    {
        /// <summary>
        /// Codeforces user handle.
        /// </summary>
        public string? Handle { get; set; }
        public override string? ToString() => Handle;

        public override bool Equals(object? obj)
        {
            if (obj is MemberDto dto) { return Equals(dto); }
            return false;
        }

        public override int GetHashCode() => Handle?.GetHashCode() ?? 0;

        public bool Equals(MemberDto? other)
        {
            if (other == null) { return false; }
            return Handle?.Equals(other.Handle, StringComparison.OrdinalIgnoreCase) ?? other.Handle == null;
        }
    }
}