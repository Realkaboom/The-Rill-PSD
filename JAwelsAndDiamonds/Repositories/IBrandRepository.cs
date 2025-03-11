using System.Collections.Generic;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public interface IBrandRepository : IRepository<Brand>
    {
        /// <summary>
        /// Gets brands by class (Regular, Premium, Luxury)
        /// </summary>
        /// <param name="brandClass">The class to filter by</param>
        /// <returns>IEnumerable of brands in the specified class</returns>
        IEnumerable<Brand> GetBrandsByClass(string brandClass);
    }
}