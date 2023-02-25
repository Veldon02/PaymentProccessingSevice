using Newtonsoft.Json;

namespace Application.Models
{
    public class CityStatistic
    {
        [JsonProperty("city")]
        public string City { get; set; } = null!;
        [JsonProperty("services")] 
        public List<ServiceStatistic> Services { get; set; } = null!;
        [JsonProperty("total")]
        public decimal Total { get; set; }
    }
}
