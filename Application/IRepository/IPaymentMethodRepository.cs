using Domain.DTO;

namespace Application.IRepository
{
    public interface IPaymentMethodRepository
    {
        IQueryable<DTOPaymentMethodResponse> GetAllPayment();
        Task<DTOResponse<DTOPaymentMethodResponse>> GetById(string id);
        Task<DTOResponse<DTOPaymentMethodResponse>> CreatePayment(DTOPaymentMethodRequestCreate request);
        Task<DTOResponse<DTOPaymentMethodResponse>> UpdatePayment(DTOPaymentMethodRequestUpdate request);
        Task<DTOResponse<DTOPaymentMethodResponse>> InActivePayment(string id);
    }
}
