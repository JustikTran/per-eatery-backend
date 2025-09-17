using Domain.DTO;

namespace Api.VNPayService
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(DTOVNPay model, HttpContext context);
        DTOVNPayResponse PaymentExecute(IQueryCollection collections);
    }
}
