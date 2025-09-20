using Domain.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class DTOAddressReceiveResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(Receiver))]
        public string? Receiver { get; set; }
        [JsonPropertyName(nameof(Phone))]
        public string? Phone { get; set; }
        [JsonPropertyName(nameof(Address))]
        public string? Address { get; set; }
        [JsonPropertyName(nameof(IsDefault))]
        public bool IsDefault { get; set; }
        [JsonPropertyName(nameof(UserId))]
        public string? UserId { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
    }

    public class DTOAddressRequestCreate
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string Receiver { get; set; } = string.Empty;
        [Required]
        [StringLength(15)]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [StringLength(150)]
        public string Address { get; set; } = string.Empty;
        [DefaultValue(false)]
        public bool IsDefault { get; set; } = false;
    }

    public class DTOAddressRequestUpdate : DTOAddressRequestCreate
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }

    public class AddressMapper
    {
        private static AddressMapper? _instance;
        private static readonly object _lock = new object();
        public static AddressMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AddressMapper();
                    }
                    return _instance;
                }
            }
        }

        public AddressReceive ToEntity(DTOAddressRequestCreate requestCreate)
        {
            return new AddressReceive
            {
                Receiver = requestCreate.Receiver,
                Phone = requestCreate.Phone,
                Address = requestCreate.Address,
                IsDefault = requestCreate.IsDefault,
                UserId = Guid.Parse(requestCreate.UserId)
            };
        }

        public DTOAddressReceiveResponse ToResponse(AddressReceive entity)
        {
            return new DTOAddressReceiveResponse
            {
                Id = entity.Id.ToString(),
                Receiver = entity.Receiver,
                Phone = entity.Phone,
                Address = entity.Address,
                IsDefault = entity.IsDefault,
                UserId = entity.UserId.ToString(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
