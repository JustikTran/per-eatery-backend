using Domain.DTO;

namespace Application.IRepository
{
    public interface IOrderRepository
    {
        IQueryable<DTOOrderResponse> GetAllOrder();
        Task<DTOResponse<DTOOrderResponse>> GetById(string orderId);
        Task<DTOResponse<DTOOrderResponse>> CreateOrder(DTOOrderRequestCreate order);
        Task<DTOResponse<DTOOrderResponse>> UpdateOrder(DTOOrderRequestUpdate order);
        Task<DTOResponse<object>> DeleteOrder(string orderId);
    }
}
