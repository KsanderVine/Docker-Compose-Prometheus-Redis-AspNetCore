namespace UrlSaver.Dtos
{
    public class UrlReadDto
    {
        public Guid Id { get; set; }
        public string Original { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
