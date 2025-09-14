using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class User : BaseEntity
    {
        [Required]
        [Column(TypeName = "VARCHAR(30)")]
        public string Username { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(65)")]
        public string Password { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Email { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(15)")]
        public string Phone { get; set; } = default!;

        [Column(TypeName = "VARCHAR(10)")]
        public string Role { get; set; } = default!;

        [Column(TypeName = "BOOLEAN")]
        public bool IsBanned { get; set; }

        [Column(TypeName = "BOOLEAN")]
        public bool Actived { get; set; }

        [Column(TypeName = "BOOLEAN")]
        public bool IsDeleted { get; set; }

        public virtual Profile Profile { get; set; } = default!;
    }
}
