using System.Collections.Generic;
using System.Linq;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public interface IJewelRepository : IRepository<Jewel>
    {
        /// <summary>
        /// Gets jewels by category ID
        /// </summary>
        /// <param name="categoryId">Category ID to filter by</param>
        /// <returns>IEnumerable of jewels in the specified category</returns>
        IEnumerable<Jewel> GetJewelsByCategory(int categoryId);

        /// <summary>
        /// Gets jewels by brand ID
        /// </summary>
        /// <param name="brandId">Brand ID to filter by</param>
        /// <returns>IEnumerable of jewels from the specified brand</returns>
        IEnumerable<Jewel> GetJewelsByBrand(int brandId);

        /// <summary>
        /// Gets detailed information about a jewel including its category and brand
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <returns>Dynamic object with jewel details</returns>
        dynamic GetJewelDetails(int jewelId);

        /// <summary>
        /// Gets detailed information about all jewels including category and brand
        /// </summary>
        /// <returns>IQueryable of dynamic objects with jewel details</returns>
        IQueryable<dynamic> GetAllJewelDetails();
    }
}