using Microsoft.Extensions.Configuration;
using WebBeds.Integration.CheapAwesome;

namespace WebBeds.Configurations
{
    public class ConfigurationCheapBeds : IConfigurationCheapBeds
    {
        public string BaseAddress { get; }
        public string FindBargainEndPoint { get; }
        public string Code { get; }

        #region [ Resilience ]
        public int RetryCount { get; }
        public int RetryDelayFactor { get; }
        public int HandledEventsBeforeBreaking { get; }
        public int DurationOfBreakSeconds { get; }
        #endregion

        public ConfigurationCheapBeds(
            IConfiguration configuration)
        {
            BaseAddress = configuration.GetValue<string>("CheapAwesome:BaseAddress");
            FindBargainEndPoint = configuration.GetValue<string>("CheapAwesome:FindBargainEndPoint");
            Code = configuration.GetValue<string>("CheapAwesome:Code");
            RetryCount = configuration.GetValue<int>("CheapAwesome:Resilience:RetryCount");
            RetryDelayFactor = configuration.GetValue<int>("CheapAwesome:Resilience:RetryDelayFactor");
            HandledEventsBeforeBreaking = configuration.GetValue<int>("CheapAwesome:Resilience:HandledEventsBeforeBreaking");
            DurationOfBreakSeconds = configuration.GetValue<int>("CheapAwesome:Resilience:DurationOfBreakSeconds");
        }
    }
}
