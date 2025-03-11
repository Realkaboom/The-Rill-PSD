using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Controllers
{
    /// <summary>
    /// Controller for jewel-related operations
    /// </summary>
    public class JewelController
    {
        private readonly JewelHandler _jewelHandler;
        private readonly Page _page;

        /// <summary>
        /// Constructor for JewelController
        /// </summary>
        /// <param name="jewelHandler">Jewel handler</param>
        /// <param name="page">The current page</param>
        public JewelController(JewelHandler jewelHandler, Page page)
        {
            _jewelHandler = jewelHandler;
            _page = page;
        }

        /// <summary>
        /// Gets all jewels
        /// </summary>
        /// <returns>IEnumerable of all jewels</returns>
        public IEnumerable<Jewel> GetAllJewels()
        {
            return _jewelHandler.GetAllJewels();
        }

        /// <summary>
        /// Gets all jewel details including category and brand information
        /// </summary>
        /// <returns>IQueryable of jewel details</returns>
        public IQueryable<dynamic> GetAllJewelDetails()
        {
            return _jewelHandler.GetAllJewelDetails();
        }

        /// <summary>
        /// Gets a jewel by its ID
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <returns>The jewel if found, otherwise null</returns>
        public Jewel GetJewelById(int jewelId)
        {
            return _jewelHandler.GetJewelById(jewelId);
        }

        /// <summary>
        /// Gets detailed information about a jewel including its category and brand
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <returns>Dynamic object with jewel details</returns>
        public dynamic GetJewelDetails(int jewelId)
        {
            return _jewelHandler.GetJewelDetails(jewelId);
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>IEnumerable of all categories</returns>
        public IEnumerable<Category> GetAllCategories()
        {
            return _jewelHandler.GetAllCategories();
        }

        /// <summary>
        /// Gets all brands
        /// </summary>
        /// <returns>IEnumerable of all brands</returns>
        public IEnumerable<Brand> GetAllBrands()
        {
            return _jewelHandler.GetAllBrands();
        }

        /// <summary>
        /// Adds a new jewel
        /// </summary>
        /// <param name="jewelName">Jewel name</param>
        /// <param name="price">Jewel price</param>
        /// <param name="releaseYear">Release year</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="brandId">Brand ID</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the addition was successful, otherwise false</returns>
        public bool AddJewel(string jewelName, decimal price, int releaseYear, int categoryId, int brandId, out string errorMessage)
        {
            errorMessage = "";

            // Validate jewel name
            if (string.IsNullOrEmpty(jewelName) || jewelName.Length < 3 || jewelName.Length > 25)
            {
                errorMessage = "Jewel name must be between 3 to 25 characters.";
                return false;
            }

            // Validate price
            if (price <= 25)
            {
                errorMessage = "Price must be more than $25.";
                return false;
            }

            // Validate release year
            if (releaseYear > DateTime.Now.Year)
            {
                errorMessage = "Release year must be less than the current year.";
                return false;
            }

            // Add the jewel
            Jewel newJewel = _jewelHandler.AddJewel(jewelName, price, releaseYear, categoryId, brandId);
            if (newJewel == null)
            {
                errorMessage = "Failed to add the jewel. Please check the input values.";
                return false;
            }

            return true;
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
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the update was successful, otherwise false</returns>
        public bool UpdateJewel(int jewelId, string jewelName, decimal price, int releaseYear, int categoryId, int brandId, out string errorMessage)
        {
            errorMessage = "";

            // Validate jewel name
            if (string.IsNullOrEmpty(jewelName) || jewelName.Length < 3 || jewelName.Length > 25)
            {
                errorMessage = "Jewel name must be between 3 to 25 characters.";
                return false;
            }

            // Validate price
            if (price <= 25)
            {
                errorMessage = "Price must be more than $25.";
                return false;
            }

            // Validate release year
            if (releaseYear > DateTime.Now.Year)
            {
                errorMessage = "Release year must be less than the current year.";
                return false;
            }

            // Update the jewel
            bool success = _jewelHandler.UpdateJewel(jewelId, jewelName, price, releaseYear, categoryId, brandId);
            if (!success)
            {
                errorMessage = "Failed to update the jewel. Please check the input values.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes a jewel
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the deletion was successful, otherwise false</returns>
        public bool DeleteJewel(int jewelId, out string errorMessage)
        {
            errorMessage = "";

            // Delete the jewel
            bool success = _jewelHandler.DeleteJewel(jewelId);
            if (!success)
            {
                errorMessage = "Failed to delete the jewel.";
                return false;
            }

            return true;
        }
    }
}