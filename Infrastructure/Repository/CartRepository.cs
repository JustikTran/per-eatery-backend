using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext context;
        public CartRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }

        private decimal TotalPrice(Guid cartId)
        {
            var listItems = context.CartItems
                .Include(ci => ci.Dish)
                .Where(ci => ci.CartId == cartId)
                .ToList();
            decimal total = 0;
            foreach (var item in listItems)
            {
                total += item.Quantity * item.Dish.Price;
            }
            return total;
        }


        public async Task<DTOResponse<DTOCartResponse>> CreateCart(DTOCartRequestCreate requestCreate)
        {
            try
            {
                var cart = CartMapper.Instance.ToEntity(requestCreate);
                context.Carts.Add(cart);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOCartResponse>
                    {
                        Message = "Create cart successfully",
                        StatusCode = 201,
                        Data = CartMapper.Instance.ToResponse(cart)
                    };
                }
                else
                {
                    return new DTOResponse<DTOCartResponse>
                    {
                        Message = "Create cart failed",
                        StatusCode = 400,
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {

                return new DTOResponse<DTOCartResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<object>> DeleteCart(string cartId)
        {
            try
            {
                var cart = await context.Carts.FindAsync(Guid.Parse(cartId));
                if (cart == null)
                {
                    return new DTOResponse<object>
                    {
                        Message = "Cart not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                context.Carts.Remove(cart);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<object>
                    {
                        Message = "Delete cart successfully",
                        StatusCode = 200,
                        Data = null
                    };
                }
                else
                {
                    return new DTOResponse<object>
                    {
                        Message = "Delete cart failed",
                        StatusCode = 400,
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<object>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public IQueryable<DTOCartResponse> GetAllCart()
        {
            try
            {
                var listCart = context.Carts
                    .Include(c => c.CartItems)
                    .ToList();
                return listCart
                    .Select(c =>
                    {
                        var res = CartMapper.Instance.ToResponse(c);
                        res.Total = TotalPrice(c.Id);
                        return res;
                    })
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<DTOCartResponse>> GetById(string cartId)
        {
            try
            {
                var cart = await context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.Id == Guid.Parse(cartId));
                if (cart == null)
                {
                    return new DTOResponse<DTOCartResponse>
                    {
                        Message = "Cart not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                return new DTOResponse<DTOCartResponse>
                {
                    Message = "Get cart successfully",
                    StatusCode = 200,
                    Data = CartMapper.Instance.ToResponse(cart)
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOCartResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOCartResponse>> UpdateCart(DTOCartRequestUpdate requestUpdate)
        {
            try
            {
                var cart = await context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.Id == Guid.Parse(requestUpdate.Id));
                if (cart == null)
                {
                    return new DTOResponse<DTOCartResponse>
                    {
                        Message = "Cart not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                cart.Thumbnail = requestUpdate.Thumbnail;
                cart.UpdatedAt = DateTime.UtcNow;
                context.Carts.Update(cart);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOCartResponse>
                    {
                        Message = "Update cart successfully",
                        StatusCode = 200,
                        Data = CartMapper.Instance.ToResponse(cart)
                    };
                }
                else
                {
                    return new DTOResponse<DTOCartResponse>
                    {
                        Message = "Update cart failed",
                        StatusCode = 400,
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOCartResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }
    }
}
