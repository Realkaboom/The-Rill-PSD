using System;
using System.Collections.Generic;
using System.Linq;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;

namespace JAwelsAndDiamonds.Handlers
{
    /// <summary>
    /// Handler for jewel-related business logic
    /// </summary>
    public class JewelHandler
    {
        private readonly IJewelRepository _jewelRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly JewelFactory _jewelFactory;

        /// <summary>
        /// Constructor for JewelHandler
        /// </summary>
        /// <param name="jewelRepository">Jewel repository</param>
        /// <param name="categoryRepository">Category repository</param>
        /// <param name="brandRepository">Brand repository</param>
        /// <param name="jewelFactory">Jewel factory</param>
        public JewelHandler(
            IJewelRepository jewelRepository,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            JewelFactory jewelFactory)
        {
            _jewelRepository = jewelRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _jewelFactory = jewelFactory;
        }

        /// <summary>
        /// Gets all jewels
        /// </summary>
        /// <returns>IEnumerable of all jewels</returns>
        public IEnumerable<Jewel> GetAllJewels()
        {
            return _jewelRepository.GetAll();
        }

        /// <summary>
        /// Gets all jewel details including category and brand information
        /// </summary>
        /// <returns>IQueryable of jewel details</returns>
        public IQueryable<dynamic> GetAllJewelDetails()
        {
            return _jewelRepository.GetAllJewelDetails();
        }

        /// <summary>
        /// Gets a jewel by its ID
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <returns>The jewel if found, otherwise null</returns>
        public Jewel GetJewelById(int jewelId)
        {
            return _jewelRepository.GetById(jewelId);
        }

        /// <summary>
        /// Gets detailed information about a jewel including its category and brand
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <returns>Dynamic object with jewel details</returns>
        public dynamic GetJewelDetails(int jewelId)
        {
            return _jewelRepository.GetJewelDetails(jewelId);
        }

        /// <summary>
        /// Gets jewels by category
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        /// <returns>IEnumerable of jewels in the specified category</returns>
        public IEnumerable<Jewel> GetJewelsByCategory(int categoryId)
        {
            return _jewelRepository.GetJewelsByCategory(categoryId);
        }

        /// <summary>
        /// Gets jewels by brand
        /// </summary>
        /// <param name="brandId">Brand ID</param>
        /// <returns>IEnumerable of jewels from the specified brand</returns>
        public IEnumerable<Jewel> GetJewelsByBrand(int brandId)
        {
            return _jewelRepository.GetJewelsByBrand(brandId);
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>IEnumerable of all categories</returns>
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        /// <summary>
        /// Gets all brands
        /// </summary>
        /// <returns>IEnumerable of all brands</returns>
        public IEnumerable<Brand> GetAllBrands()
        {
            return _brandRepository.GetAll();
        }

        /// <summary>
        /// Adds a new jewel
        /// </summary>
        /// <param name="jewelName">Jewel name</param>
        /// <param name="price">Jewel price</param>
        /// <param name="releaseYear">Release year</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="brandId">Brand ID</param>
        /// <returns>The newly added jewel if successful, otherwise null</returns>
        public Jewel AddJewel(string jewelName, decimal price, int releaseYear, int categoryId, int brandId)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(jewelName) || jewelName.Length < 3 || jewelName.Length > 25)
                    return null;

                if (price <= 25)
                    return null;

                if (releaseYear > DateTime.Now.Year)
                    return null;

                if (_categoryRepository.GetById(categoryId) == null)
                    return null;

                if (_brandRepository.GetById(brandId) == null)
                    return null;

                // Create and add the jewel
                Jewel newJewel = _jewelFactory.CreateJewel(jewelName, price, releaseYear, categoryId, brandId);
                _jewelRepository.Add(newJewel);
                _jewelRepository.SaveChanges();

                return newJewel;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Updates an existing jewel
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="jewelName">New jewel name</param>
        /// <param name="price">New price</param>
        /// <param name="releaseYear">New release year</param>
        /// <param name="categoryId">New category ID</param>
        /// <param name="brandId">New brand ID</param>
        /// <returns>True if the update was successful, otherwise false</returns>
        public bool UpdateJewel(int jewelId, string jewelName, decimal price, int releaseYear, int categoryId, int brandId)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(jewelName) || jewelName.Length < 3 || jewelName.Length > 25)
                    return false;

                if (price <= 25)
                    return false;

                if (releaseYear > DateTime.Now.Year)
                    return false;

                if (_categoryRepository.GetById(categoryId) == null)
                    return false;

                if (_brandRepository.GetById(brandId) == null)
                    return false;

                // Get the jewel to update
                Jewel jewel = _jewelRepository.GetById(jewelId);
                if (jewel == null)
                    return false;

                // Update the jewel
                jewel.JewelName = jewelName;
                jewel.Price = price;
                jewel.ReleaseYear = releaseYear;
                jewel.CategoryId = categoryId;
                jewel.BrandId = brandId;

                _jewelRepository.Update(jewel);
                _jewelRepository.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a jewel
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <returns>True if the deletion was successful, otherwise false</returns>
        public bool DeleteJewel(int jewelId)
        {
            try
            {
                _jewelRepository.Delete(jewelId);
                _jewelRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}