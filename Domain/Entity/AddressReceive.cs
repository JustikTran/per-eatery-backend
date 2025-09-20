using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class AddressReceive : BaseEntity
    {
        [Required]
        [Column(TypeName = "VARCHAR(20)")]
        public string Receiver { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(15)")]
        public string Phone { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(150)")]
        public string Address { get; set; } = default!;

        [Required]
        public bool IsDefault { get; set; } = false;

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;

        public virtual ICollection<Order> Orders { get; set; } = default!;
    }
}
