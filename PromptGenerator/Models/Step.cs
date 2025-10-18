using System.ComponentModel.DataAnnotations;

namespace PromptGenerator.Models
{
    public class Step
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public int OrderIndex { get; set; }
    }
}
