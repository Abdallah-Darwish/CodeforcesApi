using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
namespace CodeforcesApi.Core.Entities
{
    [Table("Contest")]
    public class Contest : IEquatable<Contest>
    {
        [ExplicitKey]
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString() => Name;

        public override bool Equals(object? obj)
        {
            if (obj is Contest contest) { return Equals(contest); }
            return false;
        }

        public bool Equals(Contest? other)
        {
            if (other == null) { return false; }
            return Id == other.Id && (Name?.Equals(other.Name, StringComparison.OrdinalIgnoreCase) ?? other.Name == null);
        }

        public override int GetHashCode() => Id;
    }
}
