using Domain.DTO;

namespace Application.IRepository
{
    public interface ICartItemRepositoy
    {
        IQueryable<DTOCartItemResponse> GetAllCartItem();
        Task<DTOResponse<DTOCartItemResponse>> GetById(string cartItemId);
        Task<DTOResponse<IEnumerable<DTOCartItemResponse>>> GetByCartID(string cartId);
        Task<DTOResponse<DTOCartItemResponse>> CreateCartItem(DTOCartItemRequestCreate requestCreate);
        Task<DTOResponse<DTOCartItemResponse>> UpdateCartItem(DTOCartItemRequestUpdate requestUpdate);
        Task<DTOResponse<object>> DeleteCartItem(string cartItemId);
    }
}
