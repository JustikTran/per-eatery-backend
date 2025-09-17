using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task<DTOResponse<DTOUserResponse>> ChangePassword(DTOUserChangePassword changePassword)
        {
            try
            {
                var userExisting = await _context.Users
                    .Where(u => u.Username == changePassword.Username && !u.IsDeleted)
                    .FirstOrDefaultAsync();
                if (userExisting == null)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User not found",
                        StatusCode = 404,
                        Data = null
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, userExisting.Password))
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Old password is incorrect",
                        StatusCode = 400,
                        Data = null
                    };
                }
                if (changePassword.OldPassword == changePassword.NewPassword)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "New password must be different from old password",
                        StatusCode = 400,
                        Data = null
                    };
                }
                userExisting.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
                userExisting.UpdatedAt = DateTime.UtcNow;
                _context.Users.Update(userExisting);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Password changed successfully",
                        StatusCode = 200,
                        Data = UserMapper.Instance.ToDTOUserResponse(userExisting)
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Failed to change password",
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

        public async Task<DTOResponse<DTOUserResponse>> DeleteUser(string userId)
        {
            try
            {
                var userExisting = await _context.Users.FindAsync(Guid.Parse(userId));
                if (userExisting == null)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                userExisting.IsDeleted = true;
                userExisting.UpdatedAt = DateTime.UtcNow;
                _context.Users.Update(userExisting);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User deleted successfully",
                        StatusCode = 200,
                        Data = UserMapper.Instance.ToDTOUserResponse(userExisting)
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Failed to delete user",
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

        public async Task<DTOResponse<DTOUserResponse>> ExistEmail(string email)
        {
            try
            {
                var userExisting = await _context.Users
                    .Where(u => u.Email == email && !u.IsDeleted)
                    .FirstOrDefaultAsync();
                if (userExisting != null)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Email already exists",
                        StatusCode = 200,
                        Data = UserMapper.Instance.ToDTOUserResponse(userExisting)
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Email does not exist",
                        StatusCode = 404,
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

        public async Task<DTOResponse<DTOUserResponse>> ExistPhone(string phone)
        {
            try
            {
                var userExisting = await _context.Users
                    .Where(u => u.Phone == phone && !u.IsDeleted)
                    .FirstOrDefaultAsync();
                if (userExisting != null)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Phone number already exists",
                        StatusCode = 200,
                        Data = UserMapper.Instance.ToDTOUserResponse(userExisting)
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Phone number does not exist",
                        StatusCode = 404,
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

        public async Task<DTOResponse<DTOUserResponse>> ExistUsername(string username)
        {
            try
            {
                var userExisting = await _context.Users
                    .Where(u => u.Username == username && !u.IsDeleted)
                    .FirstOrDefaultAsync();
                if (userExisting != null)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Username already exists",
                        StatusCode = 200,
                        Data = UserMapper.Instance.ToDTOUserResponse(userExisting)
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Username does not exist",
                        StatusCode = 404,
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

        public IQueryable<DTOUserResponse> GetAllUser()
        {
            try
            {
                var listUser = _context.Users.ToList();
                return listUser
                    .Select(UserMapper.Instance.ToDTOUserResponse)
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<DTOUserResponse>> GetUserById(string userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(Guid.Parse(userId));
                if (user == null)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        StatusCode = 404,
                        Message = "User does not exist.",
                        Data = null
                    };
                }
                return new DTOResponse<DTOUserResponse>
                {
                    StatusCode = 200,
                    Message = "Find user successfully.",
                    Data = UserMapper.Instance.ToDTOUserResponse(user)
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOUserResponse>
                {
                    StatusCode = 500,
                    Message = err.Message
                };
            }
        }

        public async Task<DTOResponse<DTOUserResponse>> UpdateUser(DTOUserUpdate userUpdate)
        {
            try
            {
                var userExisting = await _context.Users.FindAsync(Guid.Parse(userUpdate.Id));
                if (userExisting == null || userExisting.IsDeleted)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                userExisting.Email = userUpdate.Email;
                userExisting.Phone = userUpdate.Phone;
                userExisting.IsBanned = userUpdate.IsBanned;
                userExisting.Actived = userUpdate.Actived;
                userExisting.IsBanned = userUpdate.IsBanned;
                userExisting.UpdatedAt = DateTime.UtcNow;
                _context.Users.Update(userExisting);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "User updated successfully",
                        StatusCode = 200,
                        Data = UserMapper.Instance.ToDTOUserResponse(userExisting)
                    };
                }
                else
                {
                    return new DTOResponse<DTOUserResponse>
                    {
                        Message = "Failed to update user",
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
