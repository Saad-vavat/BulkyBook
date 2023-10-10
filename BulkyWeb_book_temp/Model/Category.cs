using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWeb_book_temp.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [DisplayName("Display Order")]
        [Range(0, 100, ErrorMessage = "Display Order Must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
