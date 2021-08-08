using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace CodeforcesApi.Core.Models
{
    public class CodeforcesResponse<T>
    {
        public string Status { get; set; }
        [JsonIgnore]
        public bool IsOk => Status == "OK";
        public string Comment { get; set; }
        public T Result { get; set; }
    }
}
