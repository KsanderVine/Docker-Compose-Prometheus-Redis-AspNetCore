using Prometheus;
using System.Text.RegularExpressions;
using UrlSaver.Data;

namespace UrlSaver.Services
{
    public class UrlMetricsService : IUrlMetricsService
    {
        private readonly ILogger<UrlMetricsService> _logger;
        private readonly string pattern = @"http[s]?://(?:www\.)?(([\w\d\.-]+)\.([a-z]+))";

        private static readonly Counter DomainsCounter = 
            Metrics.CreateCounter("url_process_domains_counter", "The number of domains saved when processing URLs", new CounterConfiguration
            { 
                LabelNames = new string[] { "top_domain", "sub_domains", "hostname" } 
            });

        private static readonly Counter ProcessRequestsCounter =
            Metrics.CreateCounter("url_process_requests_counter", "The number of requests to processe the URL");

        private static readonly Counter TotalDomainsCounter = 
            Metrics.CreateCounter("url_process_total_domains_counter", "The number of URLs processed");

        private static readonly Counter ErrorsCounter = 
            Metrics.CreateCounter("url_process_errors_counter", "The number of errors while processing the URL");

        public UrlMetricsService(ILogger<UrlMetricsService> logger)
        {
            _logger = logger;
        }

        public (string, string, string) ProcessUrl(string url)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(url);

            var topDomain = string.Empty;
            var subDomain = string.Empty;
            var hostname = string.Empty;

            if (match.Success)
            {
                topDomain = match.Groups[3].Value;
                subDomain = match.Groups[2].Value;
                hostname = match.Groups[1].Value;

                DomainsCounter
                    .WithLabels(topDomain, subDomain, hostname)
                    .Inc();

                TotalDomainsCounter.Inc();

                _logger.LogInformation("--> Processing URL: Success!");
            }
            else
            {
                ErrorsCounter.Inc();

                _logger.LogWarning("--> Processing URL: Error");
            }

            ProcessRequestsCounter.Inc();

            return (topDomain, subDomain, hostname);
        }
    }
}
