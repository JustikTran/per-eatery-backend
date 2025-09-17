using Domain.DTO;

namespace Application.IRepository
{
    public interface IMessageRepository
    {
        IQueryable<DTOMessageResponse> GetAllMessage();
        Task<DTOResponse<IEnumerable<DTOMessageResponse>>> GetByUserId(string userId);
        Task<DTOResponse<DTOMessageResponse>> CreateMessage(DTOMessageRequestCreate messageRequest);
        Task<DTOResponse<DTOMessageResponse>> UpdateMessage(DTOMessageRequestUpdate messageRequest);
        Task<DTOResponse<DTOMessageResponse>> DeleteMessage(string id);
    }
}
