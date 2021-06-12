using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebBeds.Integration.CheapAwesome
{
    public interface IServiceCheapBeds
    {
        Task<List<HotelRates>> FindBargainAsync(
                    int destination,
                    int numberOfNights);
    }
}
