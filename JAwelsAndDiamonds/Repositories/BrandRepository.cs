using System.Collections.Generic;
using System.Linq;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        public BrandRepository(JAwelsAndDiamondsEntities context) : base(context)
        {
        }

        public IEnumerable<Brand> GetBrandsByClass(string brandClass)
        {
            return _dbSet.Where(b => b.Class == brandClass).ToList();
        }
    }
}