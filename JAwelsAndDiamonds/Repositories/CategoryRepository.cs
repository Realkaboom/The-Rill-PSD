using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(JAwelsAndDiamondsEntities context) : base(context)
        {
        }
    }
}