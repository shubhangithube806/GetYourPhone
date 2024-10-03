using System.ComponentModel.DataAnnotations;

namespace GetYourPhoneWeb.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(4000)]
        [Required]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(1, 1000000)]
        public double Price { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
