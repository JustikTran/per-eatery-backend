using Domain.DTO;

namespace Application.IRepository
{
    public interface IAddressRepository
    {
        IQueryable<DTOAddressReceiveResponse> GetAllAddress();
        Task<DTOResponse<DTOAddressReceiveResponse>> GetById(string id);
        Task<DTOResponse<DTOAddressReceiveResponse>> CreateAddress(DTOAddressRequestCreate requestCreate);
        Task<DTOResponse<DTOAddressReceiveResponse>> UpdateAddress(DTOAddressRequestUpdate requestUpdate);
        Task<DTOResponse<DTOAddressReceiveResponse>> DeleteAddress(string id);
    }
}
