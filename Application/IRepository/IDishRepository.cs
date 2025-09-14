using Domain.DTO;

namespace Application.IRepository
{
    public interface IDishRepository
    {
        IQueryable<DTODishResponse> GetAllDish();
        Task<DTOResponse<DTODishResponse>> GetById(string dishId);
        Task<DTOResponse<DTODishResponse>> CreateDish(DTODishRequestCreate requestCreate);
        Task<DTOResponse<DTODishResponse>> UpdateDish(DTODishRequestUpdate requestUpdate);
        Task<DTOResponse<DTODishResponse>> DeleteDish(string dishId);
    }
}
