using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class DTOCodeRequest
    {
        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string ToMail { get; set; } = string.Empty;
        [Required]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
    }

    public class DTOVerifyCodeRequest
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public DateTime VerifyTime { get; set; }
    }
}
