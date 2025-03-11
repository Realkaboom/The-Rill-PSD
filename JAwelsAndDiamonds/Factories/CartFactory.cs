using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Factories
{
    /// <summary>
    /// Factory for creating Cart objects
    /// </summary>
    public class CartFactory : IFactory<Cart>
    {
        /// <summary>
        /// Creates a new empty Cart instance
        /// </summary>
        /// <returns>A new Cart instance</returns>
        public Cart Create()
        {
            return new Cart();
        }

        /// <summary>
        /// Creates a new Cart item
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="quantity">Quantity of the jewel</param>
        /// <returns>A new Cart instance with the specified properties</returns>
        public Cart CreateCartItem(int userId, int jewelId, int quantity)
        {
            return new Cart
            {
                UserId = userId,
                JewelId = jewelId,
                Quantity = quantity
            };
        }
    }
}