using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebBeds.Integration.CheapAwesome
{
    public class CaHotelRates
    {
        [JsonProperty("hotel")]
        public CaHotel Hotel { get; set; }

        [JsonProperty("rates")]
        public List<CaRate> Rates { get; set; }
    }
}
