using Newtonsoft.Json;

namespace WebBeds.Integration.CheapAwesome
{
    public class CaHotel
    {
        [JsonProperty("propertyId")]
        public int PropertyID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("geoId")]
        public int GeoId { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }
    }
}
