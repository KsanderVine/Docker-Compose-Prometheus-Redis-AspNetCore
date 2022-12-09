using UrlSaver.Models;

namespace UrlSaver.Data
{
    public interface IUrlRepository : IRepository<Guid, Url>
    {
    }
}
