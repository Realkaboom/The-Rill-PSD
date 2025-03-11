using System;
using System.Collections.Generic;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;

namespace JAwelsAndDiamonds.Handlers
{
    /// <summary>
    /// Handler for cart-related business logic
    /// </summary>
    public class CartHandler
    {
        private readonly ICartRepository _cartRepository;
        private readonly IJewelRepository _jewelRepository;
        private readonly CartFactory _cartFactory;

        /// <summary>
        /// Constructor for CartHandler
        /// </summary>
        /// <param name="cartRepository">Cart repository</param>
        /// <param name="jewelRepository">Jewel repository</param>
        /// <param name="cartFactory">Cart factory</param>
        public CartHandler(
            ICartRepository cartRepository,
            IJewelRepository jewelRepository,
            CartFactory cartFactory)
        {
            _cartRepository = cartRepository;
            _jewelRepository = jewelRepository;
            _cartFactory = cartFactory;
        }

        /// <summary>
        /// Gets a user's cart
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>IEnumerable of cart items with details</returns>
        public IEnumerable<dynamic> GetUserCart(int userId)
        {
            return _cartRepository.GetCartByUserId(userId);
        }

        /// <summary>
        /// Adds a jewel to the user's cart
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="quantity">Quantity to add</param>
        /// <returns>True if the addition was successful, otherwise false</returns>
        public bool AddToCart(int userId, int jewelId, int quantity)
        {
            try
            {
                // Check if the jewel exists
                Jewel jewel = _jewelRepository.GetById(jewelId);
                if (jewel == null)
                    return false;

                // Check if the quantity is valid
                if (quantity <= 0)
                    return false;

                // Check if the item is already in the cart
                Cart existingItem = _cartRepository.GetCartItemByUserAndJewel(userId, jewelId);
                if (existingItem != null)
                {
                    // Update the quantity
                    existingItem.Quantity += quantity;
                    _cartRepository.Update(existingItem);
                }
                else
                {
                    // Add a new cart item
                    Cart newCartItem = _cartFactory.CreateCartItem(userId, jewelId, quantity);
                    _cartRepository.Add(newCartItem);
                }

                _cartRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the quantity of an item in the cart
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="quantity">New quantity</param>
        /// <returns>True if the update was successful, otherwise false</returns>
        public bool UpdateCartItem(int userId, int jewelId, int quantity)
        {
            try
            {
                // Check if the quantity is valid
                if (quantity <= 0)
                    return false;

                // Get the cart item
                Cart cartItem = _cartRepository.GetCartItemByUserAndJewel(userId, jewelId);
                if (cartItem == null)
                    return false;

                // Update the quantity
                cartItem.Quantity = quantity;
                _cartRepository.Update(cartItem);
                _cartRepository.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an item from the cart
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="jewelId">Jewel ID</param>
        /// <returns>True if the deletion was successful, otherwise false</returns>
        public bool DeleteCartItem(int userId, int jewelId)
        {
            try
            {
                // Get the cart item
                Cart cartItem = _cartRepository.GetCartItemByUserAndJewel(userId, jewelId);
                if (cartItem == null)
                    return false;

                // Delete the cart item
                _cartRepository.Delete(cartItem);
                _cartRepository.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Clears all items from a user's cart
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>True if the operation was successful, otherwise false</returns>
        public bool ClearUserCart(int userId)
        {
            return _cartRepository.ClearCartByUserId(userId);
        }

        /// <summary>
        /// Calculates the total price of all items in the user's cart
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>The total price</returns>
        public decimal CalculateCartTotal(int userId)
        {
            return _cartRepository.GetCartTotal(userId);
        }
    }
}