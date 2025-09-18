using Domain.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class DTODishResponse
    {
        [JsonPropertyName(nameof(Id))]
        public string? Id { get; set; }
        [JsonPropertyName(nameof(Name))]
        public string? Name { get; set; }
        [JsonPropertyName(nameof(Description))]
        public string? Description { get; set; }
        [JsonPropertyName(nameof(Image))]
        public string? Image { get; set; }
        [JsonPropertyName(nameof(Price))]
        public decimal Price { get; set; }
        [JsonPropertyName(nameof(Type))]
        public string? Type { get; set; }
        [JsonPropertyName(nameof(IsDeleted))]
        public bool IsDeleted { get; set; }
        [JsonPropertyName(nameof(InStock))]
        public bool InStock { get; set; }
        [JsonPropertyName(nameof(CreatedAt))]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName(nameof(UpdatedAt))]
        public DateTime UpdatedAt { get; set; }
    }

    public class DTODishRequestCreate
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [DefaultValue(true)]
        public bool InStock { get; set; }
    }

    public class DTODishRequestUpdate : DTODishRequestCreate
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }

    public class DishMapper
    {
        private static DishMapper? instance;
        private static readonly object _lock = new object();
        public static DishMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new DishMapper();
                    }
                    return instance;
                }
            }
        }

        public Dish ToEntity(DTODishRequestCreate requestCreate)
        {
            return new Dish
            {
                Name = requestCreate.Name,
                Description = requestCreate.Description,
                Image = requestCreate.Image,
                Price = requestCreate.Price,
                Type = requestCreate.Type,
                IsDeleted = requestCreate.IsDeleted,
                InStock = requestCreate.InStock,
            };
        }

        public DTODishResponse ToResponse(Dish dish)
        {
            return new DTODishResponse
            {
                Id = dish.Id.ToString(),
                Name = dish.Name,
                Description = dish.Description,
                Image = dish.Image,
                Price = dish.Price,
                Type = dish.Type,
                IsDeleted = dish.IsDeleted,
                InStock = dish.InStock,
                CreatedAt = dish.CreatedAt,
                UpdatedAt = dish.UpdatedAt,
            };
        }
    }
}
