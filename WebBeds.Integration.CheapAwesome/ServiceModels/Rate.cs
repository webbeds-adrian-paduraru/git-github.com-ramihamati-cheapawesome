using Newtonsoft.Json;

namespace WebBeds.Integration.CheapAwesome
{
    public class Rate
    {
        public BoardType BoardType { get; set; }
        public RateType RateType { get; set; }
        public double Value { get; set; }
        public double FinalPrice { get; set; }
    }
}
