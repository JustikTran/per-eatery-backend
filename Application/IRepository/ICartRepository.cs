using Domain.DTO;

namespace Application.IRepository
{
    public interface ICartRepository
    {
        IQueryable<DTOCartResponse> GetAllCart();
        Task<DTOResponse<DTOCartResponse>> GetById(string cartId);
        Task<DTOResponse<DTOCartResponse>> CreateCart(DTOCartRequestCreate requestCreate);
        Task<DTOResponse<DTOCartResponse>> UpdateCart(DTOCartRequestUpdate requestUpdate);
        Task<DTOResponse<object>> DeleteCart(string cartId);
    }
}
