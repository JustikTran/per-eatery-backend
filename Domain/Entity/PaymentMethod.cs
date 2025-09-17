using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class PaymentMethod : BaseEntity
    {
        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string Name { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Description { get; set; } = default!;

        [Required]
        [Column(TypeName = "BOOLEAN")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column(TypeName = "VARCHAR(10)")]
        public string Code { get; set; } = default!;

        public virtual ICollection<Order> Orders { get; set; } = default!;
    }
}
