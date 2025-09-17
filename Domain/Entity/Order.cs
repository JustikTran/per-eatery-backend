using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Order : BaseEntity
    {
        [Required]
        [Column(TypeName = "UUID")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = default!;

        [Required]
        [Column(TypeName = "UUID")]
        public Guid PaymentMethodId { get; set; }

        [ForeignKey(nameof(PaymentMethodId))]
        public virtual PaymentMethod PaymentMethod { get; set; } = default!;

        [Column(TypeName = "TIMESTAMP WITH TIME ZONE")]
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "VARCHAR(20)")]
        public string Status { get; set; } = default!;

        [Required]
        [Column(TypeName = "BOOLEAN")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = default!;
    }
}
