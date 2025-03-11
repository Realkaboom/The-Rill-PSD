using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(JAwelsAndDiamondsEntities context) : base(context)
        {
        }

        public IEnumerable<dynamic> GetCartByUserId(int userId)
        {
            // Using the vw_CartDetails view to get detailed information
            var cartDetails = _context.Database.SqlQuery<vw_CartDetails>(
                "SELECT * FROM vw_CartDetails WHERE UserId = @userId",
                new SqlParameter("@userId", userId)
            ).ToList();

            return cartDetails;
        }

        public Cart GetCartItemByUserAndJewel(int userId, int jewelId)
        {
            return _dbSet.FirstOrDefault(c => c.UserId == userId && c.JewelId == jewelId);
        }

        public decimal GetCartTotal(int userId)
        {
            try
            {
                // Execute the stored procedure to get user cart with total
                var totalParameter = new SqlParameter
                {
                    ParameterName = "@TotalPrice",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Output,
                    Precision = 18,
                    Scale = 2
                };

                // Perhatikan bahwa stored procedure hanya membutuhkan satu parameter: @UserId
                _context.Database.ExecuteSqlCommand(
                    "EXEC sp_GetUserCart @UserId, @TotalPrice OUTPUT",
                    new SqlParameter("@UserId", userId),
                    totalParameter
                );

                // Get the result from the output parameter
                if (totalParameter.Value != DBNull.Value)
                {
                    return (decimal)totalParameter.Value;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public bool ClearCartByUserId(int userId)
        {
            try
            {
                // Get all cart items for the user
                var cartItems = _dbSet.Where(c => c.UserId == userId).ToList();

                // Remove each item
                foreach (var item in cartItems)
                {
                    _dbSet.Remove(item);
                }

                // Save changes
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}