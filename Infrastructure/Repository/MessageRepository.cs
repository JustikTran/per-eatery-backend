using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext context;
        public MessageRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<DTOResponse<DTOMessageResponse>> CreateMessage(DTOMessageRequestCreate messageRequest)
        {
            try
            {
                var newMessage = MessageMapper.Instance.ToEntity(messageRequest);
                context.Messages.Add(newMessage);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOMessageResponse>
                    {
                        StatusCode = 201,
                        Message = "Create message successfully",
                        Data = MessageMapper.Instance.ToResponse(newMessage)
                    };
                }
                else
                {
                    return new DTOResponse<DTOMessageResponse>
                    {
                        StatusCode = 400,
                        Message = "Create message failed",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOMessageResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOMessageResponse>> DeleteMessage(string id)
        {
            try
            {
                var existing = await context.Messages.FindAsync(id);
                if (existing == null)
                {
                    return new DTOResponse<DTOMessageResponse>
                    {
                        StatusCode = 404,
                        Message = "Message not found",
                        Data = null
                    };
                }
                context.Messages.Remove(existing);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOMessageResponse>
                    {
                        StatusCode = 200,
                        Message = "Delete message successfully",
                        Data = MessageMapper.Instance.ToResponse(existing)
                    };
                }
                else
                {
                    return new DTOResponse<DTOMessageResponse>
                    {
                        StatusCode = 400,
                        Message = "Delete message failed",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOMessageResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public IQueryable<DTOMessageResponse> GetAllMessage()
        {
            try
            {
                return context.Messages
                    .Select(MessageMapper.Instance.ToResponse)
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<IEnumerable<DTOMessageResponse>>> GetByUserId(string userId)
        {
            try
            {
                var listMessages = await context.Messages
                    .Where(m => m.UserId.ToString() == userId)
                    .ToListAsync();
                return new DTOResponse<IEnumerable<DTOMessageResponse>>
                {
                    StatusCode = 200,
                    Message = "Get messages successfully",
                    Data = listMessages.Select(MessageMapper.Instance.ToResponse)
                };
            }
            catch (Exception err)
            {
                return new DTOResponse<IEnumerable<DTOMessageResponse>>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTOMessageResponse>> UpdateMessage(DTOMessageRequestUpdate messageRequest)
        {
            try
            {
                var existing = await context.Messages.FindAsync(messageRequest.Id);
                if (existing == null)
                {
                    return new DTOResponse<DTOMessageResponse>
                    {
                        StatusCode = 404,
                        Message = "Message not found",
                        Data = null
                    };
                }


                existing.Message = messageRequest.Message;
                existing.Status = messageRequest.Status;
                existing.UpdatedAt = DateTime.UtcNow;
                context.Messages.Update(existing);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTOMessageResponse>
                    {
                        StatusCode = 200,
                        Message = "Update message successfully",
                        Data = MessageMapper.Instance.ToResponse(existing)
                    };
                }
                else
                {
                    return new DTOResponse<DTOMessageResponse>
                    {
                        StatusCode = 400,
                        Message = "Update message failed",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTOMessageResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }
    }
}
