using System.ComponentModel.DataAnnotations;

namespace PromptGenerator.Models
{
    public class ApplicationDomain
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
