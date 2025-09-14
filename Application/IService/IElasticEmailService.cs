using Domain.DTO;
using Domain.Entity;

namespace Application.IService
{
    public interface IElasticEmailService
    {
        Task SendEmailAsync(VerifyCode verifyCode, string templatePath );
    }
}
