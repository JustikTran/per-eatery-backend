using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class DTOProfileResponse
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
        public DateOnly Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class DTOProfileRequest
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;
        [Url]
        public string Avatar { get; set; } = string.Empty;
        [Required]
        public DateOnly Birthdate { get; set; }
    }

    public class ProfileMapper
    {
        private static ProfileMapper? instance;
        private static readonly object _lock = new object();

        public static ProfileMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new ProfileMapper();
                    }
                    return instance;
                }
            }
        }

        public DTOProfileResponse ToResponse(Profile profile)
        {
            return new DTOProfileResponse
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Avatar = profile.Avatar,
                Birthdate = profile.Birthdate,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt
            };
        }

        public Profile ToProfile(DTOProfileRequest request)
        {
            return new Profile
            {
                Id = Guid.Parse(request.Id),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Avatar = request.Avatar,
                Birthdate = request.Birthdate
            };
        }
    }
}
