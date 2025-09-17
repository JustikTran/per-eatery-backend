using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly AppDbContext context;
        public PaymentMethodRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<DTOResponse<DTOPaymentMethodResponse>> CreatePayment(DTOPaymentMethodRequestCreate request)
        {
            try
            {
                var newPayment = PaymentMethodMapper.Instance.ToEntity(request);
                context.PaymentMethods.Add(newPayment);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    var response = PaymentMethodMapper.Instance.ToResponse(newPayment);
                    return new DTOResponse<DTOPaymentMethodResponse>
                    {
                        Message = "Create payment method successfully",
                        Data = response,
                        StatusCode = 201
                    };
                }
                return new DTOResponse<DTOPaymentMethodResponse>
                {
                    Message = "Create payment method failed",
                    Data = null,
                    StatusCode = 400
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOPaymentMethodResponse>
                {
                    Message = err.Message,
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public IQueryable<DTOPaymentMethodResponse> GetAllPayment()
        {
            try
            {
                return context.PaymentMethods
                    .Select(PaymentMethodMapper.Instance.ToResponse)
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<DTOPaymentMethodResponse>> GetById(string id)
        {
            try
            {
                var payment = await context.PaymentMethods.FindAsync(id);
                if (payment == null)
                {
                    return new DTOResponse<DTOPaymentMethodResponse>
                    {
                        Message = "Payment method not found",
                        Data = null,
                        StatusCode = 404
                    };
                }
                var response = PaymentMethodMapper.Instance.ToResponse(payment);
                return new DTOResponse<DTOPaymentMethodResponse>
                {
                    Message = "Get payment method successfully",
                    Data = response,
                    StatusCode = 200
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOPaymentMethodResponse>
                {
                    Message = err.Message,
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<DTOResponse<DTOPaymentMethodResponse>> InActivePayment(string id)
        {
            try
            {
                var payment = await context.PaymentMethods.FindAsync(id);
                if (payment == null)
                {
                    return new DTOResponse<DTOPaymentMethodResponse>
                    {
                        Message = "Payment method not found",
                        Data = null,
                        StatusCode = 404
                    };
                }
                payment.IsActive = false;
                payment.UpdatedAt = DateTime.UtcNow;
                context.PaymentMethods.Update(payment);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    var response = PaymentMethodMapper.Instance.ToResponse(payment);
                    return new DTOResponse<DTOPaymentMethodResponse>
                    {
                        Message = "InActive payment method successfully",
                        Data = response,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new DTOResponse<DTOPaymentMethodResponse>
                    {
                        Message = "InActive payment method failed",
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOPaymentMethodResponse>
                {
                    Message = err.Message,
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<DTOResponse<DTOPaymentMethodResponse>> UpdatePayment(DTOPaymentMethodRequestUpdate request)
        {
            try
            {
                var existing = await context.PaymentMethods.FindAsync(request.Id);
                if (existing == null)
                {
                    return new DTOResponse<DTOPaymentMethodResponse>
                    {
                        Message = "Payment method not found",
                        Data = null,
                        StatusCode = 404
                    };
                }
                existing.Name = request.Name;
                existing.Description = request.Description;
                existing.Code = request.Code;
                existing.IsActive = request.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;
                context.PaymentMethods.Update(existing);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    var response = PaymentMethodMapper.Instance.ToResponse(existing);
                    return new DTOResponse<DTOPaymentMethodResponse>
                    {
                        Message = "Update payment method successfully",
                        Data = response,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new DTOResponse<DTOPaymentMethodResponse>
                    {
                        Message = "Update payment method failed",
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOPaymentMethodResponse>
                {
                    Message = err.Message,
                    Data = null,
                    StatusCode = 500
                };
            }
        }
    }
}
