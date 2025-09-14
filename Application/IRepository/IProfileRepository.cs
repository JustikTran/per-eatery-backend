using Domain.DTO;

namespace Application.IRepository
{
    public interface IProfileRepository
    {
        IQueryable<DTOProfileResponse> GetAllProfile();
        Task<DTOResponse<DTOProfileResponse>> GetById(string profileId);
        Task<DTOResponse<DTOProfileResponse>> CreateProfile(DTOProfileRequest profileRequest);
        Task<DTOResponse<DTOProfileResponse>> UpdateProfile(DTOProfileRequest profileRequest);
        Task<DTOResponse<DTOProfileResponse>> DeleteProfile(string profileId);
    }
}
