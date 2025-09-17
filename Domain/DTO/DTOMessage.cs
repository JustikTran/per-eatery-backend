using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class DTOMessageResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(UserId))]
        public string? UserId { get; set; }
        [JsonPropertyName(nameof(ReceiverId))]
        public string? ReceiverId { get; set; }
        [JsonPropertyName(nameof(Message))]
        public string? Message { get; set; }
        [JsonPropertyName(nameof(Status))]
        public string? Status { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
    }

    public class DTOMessageRequestCreate
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string ReceiverId { get; set; } = string.Empty;
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Message { get; set; } = string.Empty;
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string Status { get; set; } = string.Empty;
    }
    public class DTOMessageRequestUpdate : DTOMessageRequestCreate
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }

    public class MessageMapper
    {
        private static MessageMapper? _instance;
        private static readonly object _lock = new object();
        public static MessageMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MessageMapper();
                    }
                    return _instance;
                }
            }
        }
        public Messages ToEntity(DTOMessageRequestCreate requestCreate)
        {
            return new Messages
            {
                UserId = Guid.Parse(requestCreate.UserId),
                RecevierId = Guid.Parse(requestCreate.ReceiverId),
                Message = requestCreate.Message,
                Status = requestCreate.Status
            };
        }

        public DTOMessageResponse ToResponse(Messages message)
        {
            return new DTOMessageResponse
            {
                Id = message.Id.ToString(),
                UserId = message.UserId.ToString(),
                ReceiverId = message.RecevierId.ToString(),
                Message = message.Message,
                Status = message.Status,
                CreatedAt = message.CreatedAt,
                UpdatedAt = message.UpdatedAt
            };
        }
    }
}
