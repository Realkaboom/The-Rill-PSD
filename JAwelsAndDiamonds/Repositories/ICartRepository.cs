using System.Collections.Generic;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        /// <summary>
        /// Gets cart items by user ID
        /// </summary>
        /// <param name="userId">User ID to filter by</param>
        /// <returns>IEnumerable of cart items for the specified user</returns>
        IEnumerable<dynamic> GetCartByUserId(int userId);

        /// <summary>
        /// Gets a cart item by user ID and jewel ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="jewelId">Jewel ID</param>
        /// <returns>The cart item if found, otherwise null</returns>
        Cart GetCartItemByUserAndJewel(int userId, int jewelId);

        /// <summary>
        /// Gets the total price of all items in the user's cart
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Total price of the cart</returns>
        decimal GetCartTotal(int userId);

        /// <summary>
        /// Clears all items from a user's cart
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>True if successful, otherwise false</returns>
        bool ClearCartByUserId(int userId);
    }
}