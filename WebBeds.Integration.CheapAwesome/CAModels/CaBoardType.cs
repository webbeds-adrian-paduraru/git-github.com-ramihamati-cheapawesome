using System.Net.Http;
using System.Runtime.Serialization;

namespace WebBeds.Integration.CheapAwesome
{
    public enum CaBoardType
    {
        [EnumMember(Value = "Full Board")]
        FullBoard,

        [EnumMember(Value = "Half Board")]
        HalfBoard,

        [EnumMember(Value = "No Meals")]
        NoMeals
    }
}
