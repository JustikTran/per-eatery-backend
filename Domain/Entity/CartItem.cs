using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class CartItem : BaseEntity
    {
        [Required]
        [Column(TypeName = "UUID")]
        public Guid DishId { get; set; }
        [ForeignKey(nameof(DishId))]
        public virtual Dish Dish { get; set; } = default!;
        [Required]
        [Column(TypeName = "INTEGER")]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "TEXT")]
        public string Thumbnail { get; set; } = default!;
        [Required]
        [Column(TypeName = "UUID")]
        public Guid CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public virtual Cart Cart { get; set; } = default!;
    }
}
