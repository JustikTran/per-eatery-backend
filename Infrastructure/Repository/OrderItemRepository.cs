using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext context;
        public OrderItemRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<DTOResponse<DTOOrderItemResponse>> CreateOrderItem(DTOOrderItemRequestCreate orderItem)
        {
            try
            {
                var newItem = OrderItemMapper.Instance.ToEntity(orderItem);
                context.OrderItems.Add(newItem);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOOrderItemResponse>
                    {
                        StatusCode = 201,
                        Message = "Create order item successfully",
                        Data = OrderItemMapper.Instance.ToResponse(newItem)
                    };
                }
                else
                {
                    return new DTOResponse<DTOOrderItemResponse>
                    {
                        StatusCode = 400,
                        Message = "Create order item failed",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOOrderItemResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public IQueryable<DTOOrderItemResponse> GetAllOrderItems()
        {
            try
            {
                return context.OrderItems
                    .Include(oi => oi.Order)
                    .Where(oi => !oi.Order.IsDeleted)
                    .Select(OrderItemMapper.Instance.ToResponse)
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<DTOOrderItemResponse>> GetById(string orderItemId)
        {
            try
            {
                var orderItem = await context.OrderItems
                    .Include(oi => oi.Order)
                    .Where(oi => !oi.Order.IsDeleted && oi.Id.ToString() == orderItemId)
                    .FirstOrDefaultAsync();
                if (orderItem == null)
                {
                    return new DTOResponse<DTOOrderItemResponse>
                    {
                        StatusCode = 404,
                        Message = "Order item not found",
                        Data = null
                    };
                }
                return new DTOResponse<DTOOrderItemResponse>
                {
                    StatusCode = 200,
                    Message = "Get order item successfully",
                    Data = OrderItemMapper.Instance.ToResponse(orderItem)
                };
            }
            catch (Exception err)
            {

                return new DTOResponse<DTOOrderItemResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<IEnumerable<DTOOrderItemResponse>>> GetByOrderID(string orderId)
        {
            try
            {
                var orderItem = await context.OrderItems
                    .Include(oi => oi.Order)
                    .Where(oi => !oi.Order.IsDeleted && oi.OrderId.ToString() == orderId)
                    .ToListAsync();
                if (orderItem == null || orderItem.Count == 0)
                {
                    return new DTOResponse<IEnumerable<DTOOrderItemResponse>>
                    {
                        StatusCode = 404,
                        Message = "Order items not found",
                        Data = null
                    };
                }
                return new DTOResponse<IEnumerable<DTOOrderItemResponse>>
                {
                    StatusCode = 200,
                    Message = "Get order items successfully",
                    Data = orderItem.Select(OrderItemMapper.Instance.ToResponse)
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<IEnumerable<DTOOrderItemResponse>>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOOrderItemResponse>> UpdateOrderItem(DTOOrderItemRequestUpdate orderItem)
        {
            try
            {
                var existing = await context.OrderItems
                    .Include(oi => oi.Order)
                    .Where(oi => !oi.Order.IsDeleted && oi.Id.ToString() == orderItem.Id)
                    .FirstOrDefaultAsync();
                if (existing == null)
                {
                    return new DTOResponse<DTOOrderItemResponse>
                    {
                        StatusCode = 404,
                        Message = "Order item not found",
                        Data = null
                    };
                }

                existing.Quantity = orderItem.Quantity;
                existing.UnitPrice = orderItem.UnitPrice;
                existing.UpdatedAt = DateTime.UtcNow;
                context.OrderItems.Update(existing);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOOrderItemResponse>
                    {
                        StatusCode = 200,
                        Message = "Update order item successfully",
                        Data = OrderItemMapper.Instance.ToResponse(existing)
                    };
                }
                else
                {
                    return new DTOResponse<DTOOrderItemResponse>
                    {
                        StatusCode = 400,
                        Message = "Update order item failed",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOOrderItemResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }
    }
}
