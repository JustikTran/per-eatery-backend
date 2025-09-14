using Domain.DTO;
using Domain.Entity;

namespace Application.IRepository
{
    public interface IVerifyCodeRepository
    {
        Task<VerifyCode?> SaveCode(DTOCodeRequest codeRequest);
        Task DeleteCode(string codeId);
        Task<DTOResponse<object>> VerifyCode(DTOVerifyCodeRequest verifyCodeRequest);
    }
}
