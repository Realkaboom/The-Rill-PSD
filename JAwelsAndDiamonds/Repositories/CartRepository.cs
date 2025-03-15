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
            try
            {
                // Pendekatan alternatif: Membuat query dengan SELECT explisit untuk memastikan semua field tersedia
                var cartItems = _context.Database.SqlQuery<CartItem>(
                    @"SELECT c.CartId, c.UserId, c.JewelId, j.JewelName, j.Price, b.BrandName, 
                     c.Quantity, (j.Price * c.Quantity) AS Subtotal
                     FROM Cart c
                     INNER JOIN Jewel j ON c.JewelId = j.JewelId
                     INNER JOIN Brand b ON j.BrandId = b.BrandId
                     WHERE c.UserId = @userId",
                    new SqlParameter("@userId", userId)
                ).ToList();

                return cartItems;
            }
            catch (Exception ex)
            {
                // Log error jika diperlukan
                System.Diagnostics.Debug.WriteLine($"Error in GetCartByUserId: {ex.Message}");
                return new List<CartItem>();
            }
        }

        public Cart GetCartItemByUserAndJewel(int userId, int jewelId)
        {
            return _dbSet.FirstOrDefault(c => c.UserId == userId && c.JewelId == jewelId);
        }

        public decimal GetCartTotal(int userId)
        {
            try
            {
                // Gunakan query SQL langsung untuk menghitung total
                var result = _context.Database.SqlQuery<decimal>(
                    @"SELECT ISNULL(SUM(j.Price * c.Quantity), 0)
                     FROM Cart c
                     INNER JOIN Jewel j ON c.JewelId = j.JewelId
                     WHERE c.UserId = @userId",
                    new SqlParameter("@userId", userId)
                ).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                // Log error jika diperlukan
                System.Diagnostics.Debug.WriteLine($"Error in GetCartTotal: {ex.Message}");
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
            catch (Exception ex)
            {
                // Log error jika diperlukan
                System.Diagnostics.Debug.WriteLine($"Error in ClearCartByUserId: {ex.Message}");
                return false;
            }
        }

        public bool HasItems(int userId)
        {
            // Periksa apakah ada item di keranjang untuk user ini
            return _dbSet.Any(c => c.UserId == userId);
        }
    }

    // DTO untuk hasil query cart
    public class CartItem
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int JewelId { get; set; }
        public string JewelName { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}