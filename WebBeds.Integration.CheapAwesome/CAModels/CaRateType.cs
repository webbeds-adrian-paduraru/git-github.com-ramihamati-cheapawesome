using System.Runtime.Serialization;

namespace WebBeds.Integration.CheapAwesome
{

    public enum CaRateType
    {
        [EnumMember(Value = "PerNight")]
        PerNight,

        [EnumMember(Value = "Stay")]
        Stay
    }
}
