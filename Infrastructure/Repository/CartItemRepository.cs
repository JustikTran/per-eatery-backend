using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CartItemRepository : ICartItemRepositoy
    {
        private readonly AppDbContext context;
        public CartItemRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<DTOResponse<DTOCartItemResponse>> CreateCartItem(DTOCartItemRequestCreate requestCreate)
        {
            try
            {
                var cart = await context.CartItems
                    .Where(ci => ci.CartId.ToString() == requestCreate.CartId
                            && ci.DishId.ToString() == requestCreate.DishId)
                    .FirstOrDefaultAsync();
                if (cart != null)
                    return await UpdateCartItem(new DTOCartItemRequestUpdate
                    {
                        Id = cart.Id.ToString(),
                        CartId = requestCreate.CartId,
                        DishId = requestCreate.DishId,
                        Thumbnail = requestCreate.Thumbnail,
                        Quantity = cart.Quantity + requestCreate.Quantity
                    });
                else
                {
                    var newCartItem = context.CartItems.Add(CartItemMapper.Instance.ToEntity(requestCreate));
                    var result = await context.SaveChangesAsync();
                    if (result > 0)
                        return new DTOResponse<DTOCartItemResponse>
                        {
                            Message = "Create cart item successfully",
                            StatusCode = 201,
                            Data = new DTOCartItemResponse
                            {
                                Id = newCartItem.Entity.Id.ToString(),
                                CartId = newCartItem.Entity.CartId.ToString(),
                                DishId = newCartItem.Entity.DishId.ToString(),
                                Thumbnail = newCartItem.Entity.Thumbnail,
                                Quantity = newCartItem.Entity.Quantity
                            }
                        };
                    else
                        return new DTOResponse<DTOCartItemResponse>
                        {
                            Message = "Create cart item failed",
                            StatusCode = 400,
                            Data = null
                        };
                }
            }
            catch (Exception err)
            {

                return new DTOResponse<DTOCartItemResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<object>> DeleteCartItem(string cartItemId)
        {
            try
            {
                var cartItem = await context.CartItems.FindAsync(Guid.Parse(cartItemId));
                if (cartItem == null)
                {
                    return new DTOResponse<object>
                    {
                        Message = "Cart item not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                context.CartItems.Remove(cartItem);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<object>
                    {
                        Message = "Delete cart item successfully",
                        StatusCode = 200,
                        Data = null
                    };
                }
                else
                {
                    return new DTOResponse<object>
                    {
                        Message = "Delete cart item failed",
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

        public IQueryable<DTOCartItemResponse> GetAllCartItem()
        {
            try
            {
                var listItem = context.CartItems.ToList();
                return listItem
                    .Select(CartItemMapper.Instance.ToResponse)
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<IEnumerable<DTOCartItemResponse>>> GetByCartID(string cartId)
        {
            try
            {
                var item = await context.CartItems
                    .Where(ci => ci.CartId.ToString() == cartId)
                    .ToListAsync();
                if (item == null)
                {
                    return new DTOResponse<IEnumerable<DTOCartItemResponse>>
                    {
                        Message = "Cart items not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                return new DTOResponse<IEnumerable<DTOCartItemResponse>>
                {
                    Message = "Get cart items successfully",
                    StatusCode = 200,
                    Data = item.Select(CartItemMapper.Instance.ToResponse)
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<IEnumerable<DTOCartItemResponse>>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOCartItemResponse>> GetById(string cartItemId)
        {
            try
            {
                var item = await context.CartItems.FindAsync(Guid.Parse(cartItemId));
                if (item == null)
                {
                    return new DTOResponse<DTOCartItemResponse>
                    {
                        Message = "Cart item not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                return new DTOResponse<DTOCartItemResponse>
                {
                    Message = "Get cart item successfully",
                    StatusCode = 200,
                    Data = CartItemMapper.Instance.ToResponse(item)
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOCartItemResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOCartItemResponse>> UpdateCartItem(DTOCartItemRequestUpdate requestUpdate)
        {
            try
            {
                var item = await context.CartItems.FindAsync(Guid.Parse(requestUpdate.Id));
                if (item == null)
                {
                    return new DTOResponse<DTOCartItemResponse>
                    {
                        Message = "Cart item not found",
                        StatusCode = 404,
                        Data = null
                    };
                }
                item.Quantity = requestUpdate.Quantity;
                item.Thumbnail = requestUpdate.Thumbnail;
                item.UpdatedAt = DateTime.UtcNow;

                context.CartItems.Update(item);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                    return new DTOResponse<DTOCartItemResponse>
                    {
                        Message = "Update cart item successfully",
                        StatusCode = 200,
                        Data = CartItemMapper.Instance.ToResponse(item)
                    };
                else
                    return new DTOResponse<DTOCartItemResponse>
                    {
                        Message = "Update cart item failed",
                        StatusCode = 400,
                        Data = null
                    };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOCartItemResponse>
                {
                    Message = err.Message,
                    StatusCode = 500,
                    Data = null
                };
            }
        }
    }
}
