using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class DTOCartResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(UserId))]
        public string? UserId { get; set; }
        [JsonPropertyName(nameof(Thumbnail))]
        public string? Thumbnail { get; set; }
        [JsonPropertyName(nameof(Items))]
        public List<DTOCartItemResponse>? Items { get; set; }
        [JsonPropertyName(nameof(Total))]
        public decimal Total { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
    }

    public class DTOCartItemResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(DishId))]
        public string? DishId { get; set; }
        [JsonPropertyName(nameof(Dish))]
        public DTODishResponse? Dish { get; set; }
        [JsonPropertyName(nameof(Thumbnail))]
        public string? Thumbnail { get; set; }
        [JsonPropertyName(nameof(Quantity))]
        public int Quantity { get; set; }
        [JsonPropertyName(nameof(CartId))]
        public string? CartId { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
    }

    public class DTOCartRequestCreate
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Thumbnail { get; set; } = string.Empty;
        [AllowNull]
        public List<DTOCartItemRequestCreate>? Items { get; set; }
    }

    public class DTOCartRequestUpdate : DTOCartRequestCreate
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }

    public class DTOCartItemRequestCreate
    {
        [AllowNull]
        public string CartId { get; set; } = string.Empty;
        [Required]
        public string DishId { get; set; } = string.Empty;
        [Required]
        public string Thumbnail { get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }

    public class DTOCartItemRequestUpdate : DTOCartItemRequestCreate
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }

    public class CartMapper
    {
        private static CartMapper? instance;
        private static readonly object _lock = new();
        public static CartMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new CartMapper();
                    }
                    return instance;
                }
            }
        }

        public Cart ToEntity(DTOCartRequestCreate dto)
        {
            return new Cart
            {
                UserId = Guid.Parse(dto.UserId),
                Thumbnail = dto.Thumbnail,
                CartItems = dto.Items?.Select(ci => new CartItem
                {
                    DishId = Guid.Parse(ci.DishId),
                    Thumbnail = ci.Thumbnail,
                    Quantity = ci.Quantity
                }).ToList() ?? new List<CartItem>()
            };
        }

        public DTOCartResponse ToResponse(Cart entity)
        {
            return new DTOCartResponse
            {
                Id = entity.Id.ToString(),
                UserId = entity.UserId.ToString(),
                Thumbnail = entity.Thumbnail,
                Items = entity.CartItems?.Select(CartItemMapper.Instance.ToResponse).ToList() ?? new List<DTOCartItemResponse>(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }

    public class CartItemMapper
    {
        private static CartItemMapper? instance;
        private static readonly object _lock = new();
        public static CartItemMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new CartItemMapper();
                    }
                    return instance;
                }
            }
        }

        public CartItem ToEntity(DTOCartItemRequestCreate dto)
        {
            return new CartItem
            {
                CartId = Guid.Parse(dto.CartId),
                DishId = Guid.Parse(dto.DishId),
                Thumbnail = dto.Thumbnail,
                Quantity = dto.Quantity
            };
        }

        public DTOCartItemResponse ToResponse(CartItem entity)
        {
            return new DTOCartItemResponse
            {
                Id = entity.Id.ToString(),
                CartId = entity.CartId.ToString(),
                DishId = entity.DishId.ToString(),
                Dish = entity.Dish != null ? DishMapper.Instance.ToResponse(entity.Dish) : null,
                Thumbnail = entity.Thumbnail,
                Quantity = entity.Quantity,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
