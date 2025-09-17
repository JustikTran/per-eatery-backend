using Domain.DTO;

namespace Application.IRepository
{
    public interface IOrderItemRepository
    {
        IQueryable<DTOOrderItemResponse> GetAllOrderItems();
        Task<DTOResponse<DTOOrderItemResponse>> GetById(string orderItemId);
        Task<DTOResponse<IEnumerable<DTOOrderItemResponse>>> GetByOrderID(string orderId);
        Task<DTOResponse<DTOOrderItemResponse>> CreateOrderItem(DTOOrderItemRequestCreate orderItem);
        Task<DTOResponse<DTOOrderItemResponse>> UpdateOrderItem(DTOOrderItemRequestUpdate orderItem);
    }
}
