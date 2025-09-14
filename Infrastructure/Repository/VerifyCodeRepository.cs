using Application.IRepository;
using Domain.DTO;
using Domain.Entity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class VerifyCodeRepository : IVerifyCodeRepository
    {
        private readonly AppDbContext context;

        public VerifyCodeRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task DeleteCode(string codeId)
        {
            try
            {
                var existCode = await context.VerifyCodes.FindAsync(Guid.Parse(codeId));
                if (existCode != null)
                {
                    context.VerifyCodes.Remove(existCode);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<VerifyCode?> SaveCode(DTOCodeRequest codeRequest)
        {
            try
            {
                var existing = await context.VerifyCodes.FirstOrDefaultAsync(code => code.UserId.ToString() == codeRequest.UserId);
                if (existing != null)
                {
                    existing.Code = Random.Shared.Next(100000, 999999).ToString();
                    context.VerifyCodes.Update(existing);
                    await context.SaveChangesAsync();
                    return existing;
                }

                var code = new VerifyCode()
                {
                    ToMail = codeRequest.ToMail,
                    Name = codeRequest.Name,
                    Subject = codeRequest.Subject,
                    UserId = Guid.Parse(codeRequest.UserId),
                };

                context.VerifyCodes.Add(code);
                await context.SaveChangesAsync();
                return code;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<DTOResponse<object>> VerifyCode(DTOVerifyCodeRequest verifyCodeRequest)
        {
            try
            {
                var code = await context.VerifyCodes.FindAsync(Guid.Parse(verifyCodeRequest.Id));
                if (code != null && code.Code.Equals(verifyCodeRequest.Code) && code.Expired >= verifyCodeRequest.VerifyTime)
                {
                    await DeleteCode(code.Id.ToString());
                    return new DTOResponse<object>
                    {
                        StatusCode = 200,
                        Message = "Verify successfully."
                    };
                }

                if (!code!.Code.Equals(verifyCodeRequest.Code))
                {
                    return new DTOResponse<object>
                    {
                        StatusCode = 400,
                        Message = "OTP does not match."
                    };
                }

                return new DTOResponse<object>
                {
                    StatusCode = 400,
                    Message = "OTP was out of expire."
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<object>
                {
                    StatusCode = 400,
                    Message = err.Message
                };
            }
        }
    }
}
