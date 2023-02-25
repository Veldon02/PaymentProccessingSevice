using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Models
{
    public class Payer
    {
        [JsonProperty("name")] 
        public string Name { get; set; } = null!;
        [JsonProperty("payment")]
        public decimal Payment { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("account_number")]
        public long AccountNumber { get; set; }
    }
        
}
