using Domain.DTO;

namespace Application.IService
{
    public interface ITokenGenerator
    {
        string GenerateToken(DTOUserResponse user);
    }
}
