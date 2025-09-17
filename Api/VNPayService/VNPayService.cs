using Domain.DTO;

namespace Api.VNPayService
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string CreatePaymentUrl(DTOVNPay model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"] ?? "SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = timeNow.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            var pay = new VNPayLibrary();
            var urlCallBack = _configuration["VNPay:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["VNPay:Version"] ?? "2.1.0");
            pay.AddRequestData("vnp_Command", _configuration["VNPay:Command"] ?? "pay");
            pay.AddRequestData("vnp_TmnCode", _configuration["VNPay:TmnCode"]!);
            pay.AddRequestData("vnp_Amount", ((long)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["VNPay:CurrCode"]!);
            pay.AddRequestData("vnp_IpAddr", model.IpAddress ?? pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["VNPay:Locale"] ?? "en");
            pay.AddRequestData("vnp_OrderInfo", model.OrderInfo);
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack!);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["VNPay:Url"]!, _configuration["VNPay:HashSecret"]!);

            return paymentUrl;
        }


        public DTOVNPayResponse PaymentExecute(IQueryCollection collections)
        {
            var pay = new VNPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["VNPay:HashSecret"]!);

            return response;
        }
    }
}
