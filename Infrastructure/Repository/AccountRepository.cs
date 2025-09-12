using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext context;
        public AccountRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<DTOResponse<DTOUserResponse>> ForgotPassword(DTOUserAuth userAuth)
        {
            try
            {
                var existing = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == userAuth.Username);
                if (existing == null || existing.IsDeleted)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User not found.",
                        StatusCode = 404,
                        Data = null
                    };
                }
                if (existing.IsBanned)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User is banned.",
                        StatusCode = 403,
                        Data = null
                    };
                }
                existing.Password = BCrypt.Net.BCrypt.HashPassword(userAuth.Password);
                existing.UpdatedAt = DateTime.UtcNow;
                context.Users.Update(existing);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    var userResponse = UserMapper.Instance.ToDTOUserResponse(existing);
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Password changed successfully.",
                        StatusCode = 200,
                        Data = userResponse
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Change password failed.",
                        StatusCode = 500,
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {

                return new DTOResponse<DTOUserResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOUserResponse>> SignIn(DTOUserAuth userAuth)
        {
            try
            {
                var existing = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == userAuth.Username
                    || u.Email == userAuth.Username
                    || u.Id.ToString() == userAuth.Username);

                if (existing == null || existing.IsDeleted)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User not found.",
                        StatusCode = 404,
                        Data = null
                    };
                }
                if (existing.IsBanned)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User is banned.",
                        StatusCode = 403,
                        Data = null
                    };
                }
                if (BCrypt.Net.BCrypt.Verify(userAuth.Password, existing.Password))
                {
                    var userResponse = UserMapper.Instance.ToDTOUserResponse(existing);
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Sign in successfully.",
                        StatusCode = 200,
                        Data = userResponse
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Username or password is incorrect.",
                        StatusCode = 401,
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOUserResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOUserResponse>> SignUp(DTOUserRequest userRequest)
        {
            try
            {
                var user = UserMapper.Instance.ToUser(userRequest);
                var existing = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == user.Username
                    || u.Email == user.Email
                    || u.Phone == user.Phone);
                if (existing != null && !existing.IsDeleted)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Username or email or phone already exists.",
                        StatusCode = 409,
                        Data = null
                    };
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                context.Users.Add(user);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    var userResponse = UserMapper.Instance.ToDTOUserResponse(user);
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Sign up successfully.",
                        StatusCode = 201,
                        Data = userResponse
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Sign up failed.",
                        StatusCode = 500,
                        Data = null
                    };
                }

            }
            catch (Exception err)
            {
                return new DTOResponse<DTOUserResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }
    }
}
