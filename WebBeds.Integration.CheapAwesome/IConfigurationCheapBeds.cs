namespace WebBeds.Integration.CheapAwesome
{
    public interface IConfigurationCheapBeds
    {
        string BaseAddress { get; }
        string Code { get; }
        int DurationOfBreakSeconds { get; }
        string FindBargainEndPoint { get; }
        int HandledEventsBeforeBreaking { get; }
        int RetryCount { get; }
        int RetryDelayFactor { get; }
    }
}