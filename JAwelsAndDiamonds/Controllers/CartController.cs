using System;
using System.Collections.Generic;
using System.Web.UI;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Controllers
{
    /// <summary>
    /// Controller for cart-related operations
    /// </summary>
    public class CartController
    {
        private readonly CartHandler _cartHandler;
        private readonly Page _page;

        /// <summary>
        /// Constructor for CartController
        /// </summary>
        /// <param name="cartHandler">Cart handler</param>
        /// <param name="page">The current page</param>
        public CartController(CartHandler cartHandler, Page page)
        {
            _cartHandler = cartHandler;
            _page = page;
        }

        /// <summary>
        /// Gets the current user's cart
        /// </summary>
        /// <returns>IEnumerable of cart items with details</returns>
        public IEnumerable<dynamic> ViewCart()
        {
            // Get the current user ID from session
            int userId = (int)SessionUtil.GetSession(_page.Session, "UserId");
            return _cartHandler.GetUserCart(userId);
        }

        /// <summary>
        /// Adds a jewel to the current user's cart
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="quantity">Quantity to add</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the addition was successful, otherwise false</returns>
        public bool AddToCart(int jewelId, int quantity, out string errorMessage)
        {
            errorMessage = "";

            // Validate quantity
            if (quantity <= 0)
            {
                errorMessage = "Quantity must be more than 0.";
                return false;
            }

            // Get the current user ID from session
            int userId = (int)SessionUtil.GetSession(_page.Session, "UserId");

            // Add the jewel to the cart
            bool success = _cartHandler.AddToCart(userId, jewelId, quantity);
            if (!success)
            {
                errorMessage = "Failed to add the jewel to the cart.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the quantity of an item in the current user's cart
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="quantity">New quantity</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the update was successful, otherwise false</returns>
        public bool UpdateCart(int jewelId, int quantity, out string errorMessage)
        {
            errorMessage = "";

            // Validate quantity
            if (quantity <= 0)
            {
                errorMessage = "Quantity must be more than 0.";
                return false;
            }

            // Get the current user ID from session
            int userId = (int)SessionUtil.GetSession(_page.Session, "UserId");

            // Update the cart item
            bool success = _cartHandler.UpdateCartItem(userId, jewelId, quantity);
            if (!success)
            {
                errorMessage = "Failed to update the cart item.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes an item from the current user's cart
        /// </summary>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the deletion was successful, otherwise false</returns>
        public bool DeleteCartItem(int jewelId, out string errorMessage)
        {
            errorMessage = "";

            // Get the current user ID from session
            int userId = (int)SessionUtil.GetSession(_page.Session, "UserId");

            // Delete the cart item
            bool success = _cartHandler.DeleteCartItem(userId, jewelId);
            if (!success)
            {
                errorMessage = "Failed to delete the cart item.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clears all items from the current user's cart
        /// </summary>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the operation was successful, otherwise false</returns>
        public bool ClearCart(out string errorMessage)
        {
            errorMessage = "";

            // Get the current user ID from session
            int userId = (int)SessionUtil.GetSession(_page.Session, "UserId");

            // Clear the cart
            bool success = _cartHandler.ClearUserCart(userId);
            if (!success)
            {
                errorMessage = "Failed to clear the cart.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the total price of all items in the current user's cart
        /// </summary>
        /// <returns>The total price</returns>
        public decimal GetCartTotal()
        {
            // Get the current user ID from session
            int userId = (int)SessionUtil.GetSession(_page.Session, "UserId");
            return _cartHandler.CalculateCartTotal(userId);
        }
    }
}