using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class OrderItem : BaseEntity
    {
        [Required]
        [Column(TypeName = "UUID")]
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; } = default!;

        [Required]
        [Column(TypeName = "UUID")]
        public Guid DishId { get; set; }

        [ForeignKey(nameof(DishId))]
        public virtual Dish Dish { get; set; } = default!;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "MONEY")]
        public decimal UnitPrice { get; set; }
    }
}
