using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WebBeds.Integration.CheapAwesome
{
    public class ServiceCheapBeds : IServiceCheapBeds
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationCheapBeds _configuration;

        public ServiceCheapBeds(
            HttpClient httpClient,
            IConfigurationCheapBeds configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<HotelRates>> FindBargainAsync(
            int destination,
            int numberOfNights)
        {
            string relativeUri = UriHelper.BuildRelative(
                pathBase: _configuration.FindBargainEndPoint,
                query: QueryString.Create(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("destinationId", destination.ToString()),
                    new KeyValuePair<string, string>("nights", numberOfNights.ToString()),
                    new KeyValuePair<string, string>("code", _configuration.Code)
                }));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, relativeUri);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // TODO: handle exception
                throw new Exception("Api exception");
            }

            List<CaHotelRates> hotels
                = await response.Content.ReadAsAsync<List<CaHotelRates>>().ConfigureAwait(false);

            int segmentSize = hotels.Count / Environment.ProcessorCount;

            // TEST WITH PARALLEL
            List<Task> segmentationTasks = new List<Task>();
            ConcurrentBag<HotelRates> hotelsWithFinalPrice = new ConcurrentBag<HotelRates>();

            Parallel.For(0, Environment.ProcessorCount, (index) =>
            {
                CaHotelRates[] segments = index == Environment.ProcessorCount - 1
                     ? hotels.ToArray()[(index * segmentSize)..]
                     : hotels.ToArray()[(index * segmentSize)..((index + 1) * segmentSize)];

                CalculateFinalPrice(segments, numberOfNights, hotelsWithFinalPrice);
            });

            return hotelsWithFinalPrice.ToList();
        }

        private static void CalculateFinalPrice(
            CaHotelRates[] caHotelRates,
            int numberOfNights,
            ConcurrentBag<HotelRates> resultBag)
        {
            foreach (CaHotelRates caHotelRate in caHotelRates)
            {
                HotelRates hotelRates = new HotelRates
                {
                    Hotel = Map(caHotelRate.Hotel),
                    Rates = caHotelRate.Rates.ConvertAll(caRate => new Rate
                    {
                        BoardType = Map(caRate.BoardType),
                        RateType = Map(caRate.RateType),
                        Value = caRate.Value,
                        FinalPrice = (caRate.RateType == CaRateType.PerNight)
                            ? caRate.Value * numberOfNights
                            : caRate.Value
                    })
                };

                resultBag.Add(hotelRates);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Hotel Map(CaHotel source)
        {
            return new Hotel
            {
                GeoId = source.GeoId,
                Name = source.Name,
                PropertyID = source.PropertyID,
                Rating = source.Rating,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BoardType Map(CaBoardType source)
        {
            return source switch
            {
                CaBoardType.FullBoard => BoardType.FullBoard,
                CaBoardType.HalfBoard => BoardType.HalfBoard,
                CaBoardType.NoMeals => BoardType.NoMeals,
                _ => throw new ArgumentOutOfRangeException("not supported")
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RateType Map(CaRateType source)
        {
            return source switch
            {
                CaRateType.PerNight => RateType.PerNight,
                CaRateType.Stay => RateType.Stay,
                _ => throw new ArgumentOutOfRangeException("not supported")
            };
        }
    }
}
