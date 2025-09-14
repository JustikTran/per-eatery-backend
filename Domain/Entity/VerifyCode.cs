using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class VerifyCode
    {
        [Key]
        [Column(TypeName = "UUID")]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; } = Random.Shared.Next(100000, 999999).ToString();

        [Required]
        [Column(TypeName = "VARCHAR(30)")]
        public string Name { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(150)")]
        public string ToMail { get; set; } = default!;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Subject { get; set; } = default!;

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = default!;

        [Column(TypeName = "TIMESTAMP WITH TIME ZONE")]
        public DateTime Expired { get; set; }
    }
}
