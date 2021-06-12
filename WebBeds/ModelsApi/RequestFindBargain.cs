using Microsoft.AspNetCore.Mvc;

namespace WebBeds.ModelsApi
{
    public class RequestFindBargain
    {
        [FromQuery(Name = "destinationId")]
        public int DestinationId { get; set; }

        [FromQuery(Name = "nights")]
        public int Nights { get; set; }
    }
}
