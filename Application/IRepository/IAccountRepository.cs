using Domain.DTO;

namespace Application.IRepository
{
    public interface IAccountRepository
    {
        Task<DTOResponse<DTOUserResponse>> SignIn(DTOUserAuth userAuth);
        Task<DTOResponse<DTOUserResponse>> SignUp(DTOUserRequest userRequest);
        Task<DTOResponse<DTOUserResponse>> ForgotPassword(DTOUserAuth userAuth);
    }
}
