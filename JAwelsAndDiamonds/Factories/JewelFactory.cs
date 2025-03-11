using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Factories
{
    /// <summary>
    /// Factory for creating Jewel objects
    /// </summary>
    public class JewelFactory : IFactory<Jewel>
    {
        /// <summary>
        /// Creates a new empty Jewel instance
        /// </summary>
        /// <returns>A new Jewel instance</returns>
        public Jewel Create()
        {
            return new Jewel();
        }

        /// <summary>
        /// Creates a new Jewel with specified properties
        /// </summary>
        /// <param name="jewelName">Name of the jewel</param>
        /// <param name="price">Price of the jewel</param>
        /// <param name="releaseYear">Release year of the jewel</param>
        /// <param name="categoryId">Category ID of the jewel</param>
        /// <param name="brandId">Brand ID of the jewel</param>
        /// <returns>A new Jewel instance with the specified properties</returns>
        public Jewel CreateJewel(string jewelName, decimal price, int releaseYear, int categoryId, int brandId)
        {
            return new Jewel
            {
                JewelName = jewelName,
                Price = price,
                ReleaseYear = releaseYear,
                CategoryId = categoryId,
                BrandId = brandId
            };
        }
    }
}