using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class DTOOrderResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(UserId))]
        public string? UserId { get; set; }
        [JsonPropertyName(nameof(MethodId))]
        public string? MethodId { get; set; }
        [JsonPropertyName(nameof(PaidAt))]
        public DateTime PaidAt { get; set; }
        [JsonPropertyName(nameof(Status))]
        public string? Status { get; set; }
        [JsonPropertyName(nameof(IsDeleted))]
        public bool IsDeleted { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
        [JsonPropertyName(nameof(OrderItems))]
        public List<DTOOrderItemResponse>? OrderItems { get; set; }
    }

    public class DTOOrderItemResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(OrderId))]
        public string? OrderId { get; set; }
        [JsonPropertyName(nameof(DishId))]
        public string? DishId { get; set; }
        [JsonPropertyName(nameof(Quantity))]
        public int Quantity { get; set; }
        [JsonPropertyName(nameof(UnitPrice))]
        public decimal UnitPrice { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
    }

    public class DTOOrderRequestCreate
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string MethodId { get; set; } = string.Empty;

        [AllowNull]
        public List<DTOOrderItemRequestCreate> OrderItems { get; set; } = new();

        [Required]
        public string Status { get; set; } = string.Empty;

        public DateTime PaidAt { get; set; }
    }

    public class DTOOrderItemRequestCreate
    {
        [AllowedValues]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        public string DishId { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(10000, double.MaxValue, ErrorMessage = "UnitPrice must be at least 10.000")]
        public decimal UnitPrice { get; set; }
    }

    public class DTOOrderRequestUpdate : DTOOrderRequestCreate
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }

    public class DTOOrderItemRequestUpdate : DTOOrderItemRequestCreate
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }

    public class OrderMapper
    {
        private static OrderMapper? _instance;
        private static readonly object _lock = new();
        public static OrderMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new OrderMapper();
                    }
                    return _instance;
                }
            }
        }

        public Order ToEntity(DTOOrderRequestCreate requestCreate)
        {
            return new Order
            {
                UserId = Guid.Parse(requestCreate.UserId),
                PaymentMethodId = Guid.Parse(requestCreate.MethodId),
                PaidAt = requestCreate.PaidAt == default ? default : requestCreate.PaidAt,
                Status = requestCreate.Status,
                IsDeleted = false,
                OrderItems = requestCreate.OrderItems?.Select(OrderItemMapper.Instance.ToEntity).ToList() ?? new List<OrderItem>()
            };
        }

        public DTOOrderResponse ToResponse(Order order)
        {
            return new DTOOrderResponse
            {
                Id = order.Id.ToString(),
                UserId = order.UserId.ToString(),
                MethodId = order.PaymentMethodId.ToString(),
                PaidAt = order.PaidAt,
                Status = order.Status,
                IsDeleted = order.IsDeleted,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems?.Select(OrderItemMapper.Instance.ToResponse).ToList()
            };
        }
    }

    public class OrderItemMapper
    {
        private static OrderItemMapper? instance;
        private static readonly object _lock = new();
        public static OrderItemMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new();
                    }
                    return instance;
                }
            }
        }

        public OrderItem ToEntity(DTOOrderItemRequestCreate requestCreate)
        {
            return new OrderItem
            {
                OrderId = Guid.Parse(requestCreate.OrderId),
                DishId = Guid.Parse(requestCreate.DishId),
                Quantity = requestCreate.Quantity,
                UnitPrice = requestCreate.UnitPrice
            };
        }

        public DTOOrderItemResponse ToResponse(OrderItem orderItem)
        {
            return new DTOOrderItemResponse
            {
                Id = orderItem.Id.ToString(),
                OrderId = orderItem.OrderId.ToString(),
                DishId = orderItem.DishId.ToString(),
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                CreatedAt = orderItem.CreatedAt,
                UpdatedAt = orderItem.UpdatedAt
            };
        }
    }
}
