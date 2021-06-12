using Newtonsoft.Json;

namespace WebBeds.Integration.CheapAwesome
{
    public class Hotel
    {
        public int PropertyID { get; set; }

        public string Name { get; set; }

        public int GeoId { get; set; }

        public int Rating { get; set; }
    }
}
