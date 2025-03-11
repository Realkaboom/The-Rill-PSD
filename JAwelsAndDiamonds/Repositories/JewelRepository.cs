using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public class JewelRepository : BaseRepository<Jewel>, IJewelRepository
    {
        public JewelRepository(JAwelsAndDiamondsEntities context) : base(context)
        {
        }

        public IEnumerable<Jewel> GetJewelsByCategory(int categoryId)
        {
            return _dbSet.Where(j => j.CategoryId == categoryId).ToList();
        }

        public IEnumerable<Jewel> GetJewelsByBrand(int brandId)
        {
            return _dbSet.Where(j => j.BrandId == brandId).ToList();
        }

        public dynamic GetJewelDetails(int jewelId)
        {
            // Using the vw_JewelDetails view to get detailed information
            var jewelDetails = _context.Database.SqlQuery<vw_JewelDetails>(
                "SELECT * FROM vw_JewelDetails WHERE JewelId = @jewelId",
                new System.Data.SqlClient.SqlParameter("@jewelId", jewelId)
            ).FirstOrDefault();

            return jewelDetails;
        }

        public IQueryable<dynamic> GetAllJewelDetails()
        {
            // Using the vw_JewelDetails view to get detailed information for all jewels
            var jewelDetails = _context.Database.SqlQuery<vw_JewelDetails>(
                "SELECT * FROM vw_JewelDetails"
            ).AsQueryable();

            return jewelDetails;
        }
    }
}