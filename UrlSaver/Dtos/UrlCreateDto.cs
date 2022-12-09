using System.ComponentModel.DataAnnotations;

namespace UrlSaver.Dtos
{
    public class UrlCreateDto
    {
        [Required]
        public string Original { get; set; } = string.Empty;
    }
}
