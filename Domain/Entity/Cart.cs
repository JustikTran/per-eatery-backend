using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Cart : BaseEntity
    {
        [Required]
        [Column(TypeName = "UUID")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = default!;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Thumbnail { get; set; } = default!;

        public virtual ICollection<CartItem> CartItems { get; set; } = default!;
    }
}
