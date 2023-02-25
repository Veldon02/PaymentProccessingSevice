using Newtonsoft.Json;

namespace Application.Models
{
    public class ServiceStatistic
    {
        [JsonProperty("name")] 
        public string Name { get; set; } = null!;
        [JsonProperty("payers")]
        public List<Payer> Payers { get; set; } = null!;
        [JsonProperty("total")]
        public decimal Total { get; set; }

    }
}
