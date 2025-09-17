using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;
        public OrderRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<DTOResponse<DTOOrderResponse>> CreateOrder(DTOOrderRequestCreate order)
        {
            try
            {
                var newOrder = OrderMapper.Instance.ToEntity(order);
                context.Orders.Add(newOrder);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOOrderResponse>
                    {
                        StatusCode = 201,
                        Message = "Create order successfully",
                        Data = OrderMapper.Instance.ToResponse(newOrder)
                    };
                }
                else
                {
                    return new DTOResponse<DTOOrderResponse>
                    {
                        StatusCode = 400,
                        Message = "Create order failed",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {

                return new DTOResponse<DTOOrderResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<object>> DeleteOrder(string orderId)
        {
            try
            {
                var order = await context.Orders.FindAsync(Guid.Parse(orderId));
                if (order == null || order.IsDeleted)
                {
                    return new DTOResponse<object>
                    {
                        StatusCode = 404,
                        Message = "Order not found",
                        Data = null
                    };
                }

                order.IsDeleted = true;
                order.UpdatedAt = DateTime.UtcNow;
                context.Orders.Update(order);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<object>
                    {
                        StatusCode = 200,
                        Message = "Delete order successfully",
                        Data = null
                    };
                }
                else
                {
                    return new DTOResponse<object>
                    {
                        StatusCode = 400,
                        Message = "Delete order failed",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<object>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public IQueryable<DTOOrderResponse> GetAllOrder()
        {
            try
            {
                var listOrder = context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => !o.IsDeleted)
                    .Select(OrderMapper.Instance.ToResponse)
                    .AsQueryable();
                return listOrder;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<DTOOrderResponse>> GetById(string orderId)
        {
            try
            {
                var order = await context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == Guid.Parse(orderId) && !o.IsDeleted);
                if (order == null)
                {
                    return new DTOResponse<DTOOrderResponse>
                    {
                        StatusCode = 404,
                        Message = "Order not found",
                        Data = null
                    };
                }
                return new DTOResponse<DTOOrderResponse>
                {
                    StatusCode = 200,
                    Message = "Get order successfully",
                    Data = OrderMapper.Instance.ToResponse(order)
                };
            }
            catch (Exception err)
            {

                return new DTOResponse<DTOOrderResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOOrderResponse>> UpdateOrder(DTOOrderRequestUpdate order)
        {
            try
            {
                var orderExisting = await context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == Guid.Parse(order.Id) && !o.IsDeleted);
                if (orderExisting == null)
                {
                    return new DTOResponse<DTOOrderResponse>
                    {
                        StatusCode = 404,
                        Message = "Order not found",
                        Data = null
                    };
                }

                orderExisting.PaidAt = order.PaidAt == default ? orderExisting.PaidAt : order.PaidAt;
                orderExisting.Status = order.Status;
                orderExisting.UpdatedAt = DateTime.UtcNow;

                context.Orders.Update(orderExisting);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOOrderResponse>
                    {
                        StatusCode = 200,
                        Message = "Update order successfully",
                        Data = OrderMapper.Instance.ToResponse(orderExisting)
                    };
                }
                else
                {
                    return new DTOResponse<DTOOrderResponse>
                    {
                        StatusCode = 400,
                        Message = "Update order failed",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOOrderResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }
    }
}
