using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Dish : BaseEntity
    {
        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string Name { get; set; } = default!;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Description { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Image { get; set; } = default!;

        [Required]
        [Column(TypeName = "MONEY")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(10)")]
        public string Type { get; set; } = default!;

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public bool InStock { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = default!;
    }
}
