using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesApi.Core.Entities
{
    /// <summary>
    /// Used to store ProblemInfo after *scrapping* it from codeforces
    /// </summary>
    [Table("ProblemInfo")]
    public class ProblemInfo
    {
        [ExplicitKey]
        public string ProblemId { get; set; }
        public string Index { get; set; }
        public string Name { get; set; }
        public int? Rating { get; set; }
    }
}
