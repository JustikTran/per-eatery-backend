using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Profile : BaseEntity
    {
        [ForeignKey(nameof(Id))]
        public virtual User User { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(30)")]
        public string FirstName { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(30)")]
        public string LastName { get; set; } = default!;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Avatar { get; set; } = default!;

        [Required]
        [Column(TypeName = "DATE")]
        public DateOnly Birthdate { get; set; }
    }
}
