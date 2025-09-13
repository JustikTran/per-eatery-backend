using Domain.DTO;

namespace Application.IRepository
{
    public interface IUserRepository
    {
        IQueryable<DTOUserResponse> GetAllUser();
        Task<DTOResponse<DTOUserResponse>> GetUserById(string userId);
        Task<DTOResponse<DTOUserResponse>> UpdateUser(DTOUserUpdate userUpdate);
        Task<DTOResponse<DTOUserResponse>> DeleteUser(string userId);
        Task<DTOResponse<DTOUserResponse>> ExistUsername(string username);
        Task<DTOResponse<DTOUserResponse>> ExistEmail(string email);
        Task<DTOResponse<DTOUserResponse>> ExistPhone(string phone);
        Task<DTOResponse<DTOUserResponse>> ChangePassword(DTOUserChangePassword changePassword);
    }
}
