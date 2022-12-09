namespace UrlSaver.Services
{
    public interface IUrlMetricsService
    {
        (string, string, string) ProcessUrl(string url);
    }
}
