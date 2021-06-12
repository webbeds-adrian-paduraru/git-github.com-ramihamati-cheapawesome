using Newtonsoft.Json;

namespace WebBeds.Integration.CheapAwesome
{
    public class CaRate
    {
        [JsonProperty("rateType")]
        public CaRateType RateType { get; set; }

        [JsonProperty("boardType")]
        public CaBoardType BoardType { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}
