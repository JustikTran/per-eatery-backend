using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext context;
        public AddressRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }

        private async Task<bool> IsExistAddress(string userId)
        {
            return await context.AddressReceives.AnyAsync(a => a.UserId.ToString() == userId);
        }

        private async Task ResetDefaultAddress(string userId)
        {
            var addresses = await context.AddressReceives
                .Where(a => a.UserId.ToString() == userId && a.IsDefault)
                .ToListAsync();
            if (!addresses.Any()) return;
            foreach (var address in addresses)
            {
                address.IsDefault = false;
                context.Update(address);
            }
            await context.SaveChangesAsync();
        }

        public async Task<DTOResponse<DTOAddressReceiveResponse>> CreateAddress(DTOAddressRequestCreate requestCreate)
        {
            try
            {
                var exist = await IsExistAddress(requestCreate.UserId);
                if (!exist)
                {
                    requestCreate.IsDefault = true;
                }
                else if (requestCreate.IsDefault)
                {
                    await ResetDefaultAddress(requestCreate.UserId);
                }
                var address = AddressMapper.Instance.ToEntity(requestCreate);
                context.AddressReceives.Add(address);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOAddressReceiveResponse>
                    {
                        StatusCode = 201,
                        Message = "Create address successfully",
                        Data = AddressMapper.Instance.ToResponse(address)
                    };
                }
                return new DTOResponse<DTOAddressReceiveResponse>
                {
                    StatusCode = 400,
                    Message = "Create address failed",
                    Data = null
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOAddressReceiveResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOAddressReceiveResponse>> DeleteAddress(string id)
        {
            try
            {
                var existing = await context.AddressReceives.FindAsync(Guid.Parse(id));
                if (existing == null)
                {
                    return new DTOResponse<DTOAddressReceiveResponse>
                    {
                        StatusCode = 404,
                        Message = "Address not found",
                        Data = null
                    };
                }
                context.AddressReceives.Remove(existing);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOAddressReceiveResponse>
                    {
                        StatusCode = 200,
                        Message = "Delete address successfully",
                        Data = AddressMapper.Instance.ToResponse(existing)
                    };
                }
                return new DTOResponse<DTOAddressReceiveResponse>
                {
                    StatusCode = 400,
                    Message = "Delete address failed",
                    Data = null
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOAddressReceiveResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public IQueryable<DTOAddressReceiveResponse> GetAllAddress()
        {
            try
            {
                var listAddress = context.AddressReceives.ToList();
                return listAddress
                    .Select(AddressMapper.Instance.ToResponse)
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<DTOAddressReceiveResponse>> GetById(string id)
        {
            try
            {
                var address = await context.AddressReceives.FindAsync(Guid.Parse(id));
                if (address == null)
                {
                    return new DTOResponse<DTOAddressReceiveResponse>
                    {
                        StatusCode = 404,
                        Message = "Address not found",
                        Data = null
                    };
                }
                return new DTOResponse<DTOAddressReceiveResponse>
                {
                    StatusCode = 200,
                    Message = "Get address successfully",
                    Data = AddressMapper.Instance.ToResponse(address)
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOAddressReceiveResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOAddressReceiveResponse>> UpdateAddress(DTOAddressRequestUpdate requestUpdate)
        {
            try
            {
                var exist = await context.AddressReceives.FindAsync(Guid.Parse(requestUpdate.Id));
                if (exist == null)
                {
                    return new DTOResponse<DTOAddressReceiveResponse>
                    {
                        StatusCode = 404,
                        Message = "Address not found",
                        Data = null
                    };
                }
                exist.Receiver = requestUpdate.Receiver;
                exist.Phone = requestUpdate.Phone;
                exist.Address = requestUpdate.Address;
                exist.IsDefault = requestUpdate.IsDefault;
                exist.UpdatedAt = DateTime.UtcNow;
                context.Update(exist);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOAddressReceiveResponse>
                    {
                        StatusCode = 200,
                        Message = "Update address successfully",
                        Data = AddressMapper.Instance.ToResponse(exist)
                    };
                }
                return new DTOResponse<DTOAddressReceiveResponse>
                {
                    StatusCode = 400,
                    Message = "Update address failed",
                    Data = null
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOAddressReceiveResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }
    }
}
