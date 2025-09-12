using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class BaseEntity
    {
        [Key]
        [Column(TypeName = "UUID")]
        public Guid Id { get; set; }

        [Column(TypeName = "TIMESTAMP WITH TIME ZONE")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "TIMESTAMP WITH TIME ZONE")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
