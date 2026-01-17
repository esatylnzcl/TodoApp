using Core.IRepositories;
using DataAccess.Context;
using Entity.Concrete;

namespace DataAccess.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
