using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Messages : BaseEntity
    {
        [Required]
        [Column(TypeName = "UUID")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = default!;

        [Required]
        [Column(TypeName = "UUID")]
        public Guid RecevierId { get; set; }

        [ForeignKey(nameof(RecevierId))]
        public virtual User Recevier { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Message { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(10)")]
        public string Status { get; set; } = default!;
    }
}
