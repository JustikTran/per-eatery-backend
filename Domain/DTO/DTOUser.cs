using Domain.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class DTOUserResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(Username))]
        public string? Username { get; set; }
        [JsonPropertyName(nameof(Email))]
        public string? Email { get; set; }
        [JsonPropertyName(nameof(Phone))]
        public string? Phone { get; set; }
        [JsonPropertyName(nameof(Role))]
        public string? Role { get; set; }
        [JsonPropertyName(nameof(IsBanned))]
        public bool IsBanned { get; set; }
        [JsonPropertyName(nameof(Actived))]
        public bool Actived { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
    }

    public class DTOUserRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [DefaultValue("User")]
        public string Role { get; set; } = string.Empty;

        [DefaultValue(false)]
        public bool IsBanned { get; set; }

        [DefaultValue(false)]
        public bool Actived { get; set; }
    }

    public class DTOUserAuth
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class DTOUserChangePassword
    {
        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UserMapper
    {
        private static UserMapper? _instance;
        private static readonly object _lock = new object();

        public static UserMapper Instance
        {
            get
            {

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserMapper();
                    }
                    return _instance;
                }

            }
        }

        public User ToUser(DTOUserRequest request)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                Phone = request.Phone,
                Role = request.Role ?? "User",
                IsBanned = request.IsBanned,
                Actived = request.Actived,
                IsDeleted = false
            };
        }

        public DTOUserResponse ToDTOUserResponse(User user)
        {
            return new DTOUserResponse
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                IsBanned = user.IsBanned,
                Actived = user.Actived,
                CreatedAt = DateTime.SpecifyKind(user.CreatedAt.ToLocalTime(), DateTimeKind.Local),
                UpdatedAt = DateTime.SpecifyKind(user.UpdatedAt.ToLocalTime(), DateTimeKind.Local),
            };
        }
    }
}
