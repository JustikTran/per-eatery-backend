using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext context;
        public ProfileRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<DTOResponse<DTOProfileResponse>> CreateProfile(DTOProfileRequest profileRequest)
        {
            try
            {
                var profile = ProfileMapper.Instance.ToProfile(profileRequest);
                context.Profiles.Add(profile);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOProfileResponse>
                    {
                        StatusCode = 201,
                        Message = "Create profile succcessfully.",
                        Data = ProfileMapper.Instance.ToResponse(profile)
                    };
                }
                else
                {
                    return new DTOResponse<DTOProfileResponse>
                    {
                        StatusCode = 500,
                        Message = "Create profile failure.",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOProfileResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOProfileResponse>> DeleteProfile(string profileId)
        {
            try
            {
                var profile = await context.Profiles.FindAsync(Guid.Parse(profileId));
                if (profile == null)
                    return new DTOResponse<DTOProfileResponse>
                    {
                        StatusCode = 500,
                        Message = "Profile does not exist.",
                        Data = null
                    };
                context.Profiles.Remove(profile);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOProfileResponse>
                    {
                        StatusCode = 200,
                        Message = "Delete profile successfully.",
                        Data = null
                    };
                }
                else
                {
                    return new DTOResponse<DTOProfileResponse>
                    {
                        StatusCode = 500,
                        Message = "Delete profile failure.",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOProfileResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public IQueryable<DTOProfileResponse> GetAllProfile()
        {
            try
            {
                var listProfile = context.Profiles.ToList();
                return listProfile
                    .Select(ProfileMapper.Instance.ToResponse)
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<DTOProfileResponse>> GetById(string profileId)
        {
            try
            {
                var profile = await context.Profiles.FindAsync(Guid.Parse(profileId));
                if (profile == null)
                {
                    return new DTOResponse<DTOProfileResponse>
                    {
                        Message = "Profile not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                return new DTOResponse<DTOProfileResponse>
                {
                    Message = "Profile found",
                    StatusCode = 200,
                    Data = ProfileMapper.Instance.ToResponse(profile)
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOProfileResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public Task<DTOResponse<DTOProfileResponse>> UpdateProfile(DTOProfileRequest profileRequest)
        {
            throw new NotImplementedException();
        }
    }
}
