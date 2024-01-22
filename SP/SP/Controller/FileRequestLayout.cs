using System.ComponentModel.DataAnnotations;

namespace SP.Controller
{
    public class FileRequestLayout
    {
        [Required]
        public string? raw { get; set; }
        [Required]
        public IFormFile? file { get; set; }
    }
}