using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeforcesApi.Core.Models;
namespace CodeforcesApi.Core.Entities
{
    public class Submission : IEquatable<Submission>
    {
        [ExplicitKey]
        public int Id { get; set; }
        public string AuthorHandle { get; set; }
        public SubmissionVerdict Verdict { get; set; }
        public string ProblemId { get; set; }

        public bool Equals(Submission? other)
        {
            if (other == null) { return false; }
            return Id == other.Id &&
                (AuthorHandle?.Equals(other.AuthorHandle, StringComparison.OrdinalIgnoreCase) ?? other.AuthorHandle == null) &&
                Verdict == other.Verdict &&
                ProblemId == other.ProblemId;

        }

        public override bool Equals(object? obj)
        {
            if (obj is Submission sub) { return Equals(sub); }
            return false;
        }

        public override int GetHashCode() => Id;

        public override string ToString() => $"{Id}|{AuthorHandle}|{ProblemId}|{Verdict}";
    }
}
