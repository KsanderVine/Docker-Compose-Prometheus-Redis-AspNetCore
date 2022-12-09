namespace UrlSaver.Models
{
    public class Url : BaseModel
    {
        public string Original { get; set; } = string.Empty;

        public string TopDomain { get; set; } = string.Empty;
        public string SubDomains { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
    }
}
