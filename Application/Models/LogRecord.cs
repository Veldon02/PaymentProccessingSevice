using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class LogRecord
    {
        [JsonProperty("parsed_files")]
        public int ParsedFiles { get; set; } 
        [JsonProperty("parsed_lines")]
        public long ParsedLines { get; set; } 
        [JsonProperty("found_errors")]
        public int FoundErrors { get; set; }
        [JsonProperty("invalid_files")]
        public List<string> InvalidFiles { get; set; } = null!;
    }
}
