using Application.IRepository;
using Domain.DTO;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class DishRepository : IDishRepository
    {
        private AppDbContext context;

        public DishRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task<DTOResponse<DTODishResponse>> CreateDish(DTODishRequestCreate requestCreate)
        {
            try
            {
                var dish = DishMapper.Instance.ToEntity(requestCreate);
                context.Dishes.Add(dish);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 201,
                        Message = "Create dish successfully.",
                        Data = DishMapper.Instance.ToResponse(dish)
                    };
                }
                else
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 500,
                        Message = "Create dish failure.",
                        Data = null
                    };
                }
            }
            catch (Exception err)
            {

                return new DTOResponse<DTODishResponse>
                {
                    StatusCode = 500,
                    Message = err.Message,
                    Data = null
                };
            }
        }

        public async Task<DTOResponse<DTODishResponse>> DeleteDish(string dishId)
        {
            try
            {
                var dish = await context.Dishes.FindAsync(Guid.Parse(dishId));
                if (dish == null || dish.IsDeleted)
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 404,
                        Message = "Dish does not be found."
                    };
                }
                dish.IsDeleted = true;
                dish.UpdatedAt = DateTime.UtcNow;
                context.Dishes.Update(dish);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 200,
                        Message = "Delete dish successfully.",
                        Data = null
                    };
                }
                else
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 500,
                        Message = "Delete dish failure."
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTODishResponse>
                {
                    StatusCode = 500,
                    Message = err.Message
                };
            }
        }

        public IQueryable<DTODishResponse> GetAllDish()
        {
            try
            {
                var listDish = context.Dishes.ToList();
                return listDish
                    .Select(DishMapper.Instance.ToResponse)
                    .AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<DTOResponse<DTODishResponse>> GetById(string dishId)
        {
            try
            {
                var dish = await context.Dishes.FindAsync(Guid.Parse(dishId));
                if (dish == null || dish.IsDeleted)
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 404,
                        Message = "Dish does not be found."
                    };
                }
                else
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 200,
                        Message = "Dish was found.",
                        Data = DishMapper.Instance.ToResponse(dish)
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTODishResponse>
                {
                    StatusCode = 500,
                    Message = err.Message
                };
            }
        }

        public async Task<DTOResponse<DTODishResponse>> UpdateDish(DTODishRequestUpdate requestUpdate)
        {
            try
            {
                var dish = await context.Dishes.FindAsync(Guid.Parse(requestUpdate.Id));
                if (dish == null || dish.IsDeleted)
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 404,
                        Message = "Dish does not be found."
                    };
                }
                dish.Name = requestUpdate.Name;
                dish.Description = requestUpdate.Description;
                dish.Image = requestUpdate.Image;
                dish.Price = requestUpdate.Price;
                dish.InStock = requestUpdate.InStock;
                dish.UpdatedAt = DateTime.UtcNow;

                context.Dishes.Update(dish);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 200,
                        Message = "Update dish successfully."
                    };
                }
                else
                {
                    return new DTOResponse<DTODishResponse>
                    {
                        StatusCode = 500,
                        Message = "Update dish failure."
                    };
                }
            }
            catch (Exception err)
            {
                return new DTOResponse<DTODishResponse>
                {
                    StatusCode = 500,
                    Message = err.Message
                };
            }
        }
    }
}
