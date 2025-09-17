using Domain.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class DTOPaymentMethodResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(Name))]
        public string? Name { get; set; }
        [JsonPropertyName(nameof(Description))]
        public string? Description { get; set; }
        [JsonPropertyName(nameof(Code))]
        public string? Code { get; set; }
        [JsonPropertyName(nameof(IsActive))]
        public bool IsActive { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
    }

    public class DTOPaymentMethodRequestCreate
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Code { get; set; } = string.Empty;
        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }

    public class DTOPaymentMethodRequestUpdate : DTOPaymentMethodRequestCreate
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }

    public class PaymentMethodMapper
    {
        private static PaymentMethodMapper? instance;
        private static readonly object _lock = new object();
        public static PaymentMethodMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new PaymentMethodMapper();
                    }
                    return instance;
                }

            }
        }

        public PaymentMethod ToEntity(DTOPaymentMethodRequestCreate requestCreate)
        {
            return new PaymentMethod
            {
                Name = requestCreate.Name,
                Description = requestCreate.Description,
                Code = requestCreate.Code,
                IsActive = requestCreate.IsActive,
            };
        }

        public DTOPaymentMethodResponse ToResponse(PaymentMethod entity)
        {
            return new DTOPaymentMethodResponse
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Description = entity.Description,
                Code = entity.Code,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            };
        }
    }
}
