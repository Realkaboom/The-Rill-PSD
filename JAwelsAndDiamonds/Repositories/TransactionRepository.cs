using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public class TransactionRepository : BaseRepository<TransactionHeader>, ITransactionRepository
    {
        public TransactionRepository(JAwelsAndDiamondsEntities context) : base(context)
        {
        }

        public IEnumerable<dynamic> GetTransactionsByUserId(int userId)
        {
            // Using the vw_TransactionDetails view to get transaction headers
            var transactions = _context.Database.SqlQuery<dynamic>(
                @"SELECT DISTINCT TransactionId, UserId, Username, TransactionDate, PaymentMethodName, Status 
                  FROM vw_TransactionDetails 
                  WHERE UserId = @userId
                  ORDER BY TransactionDate DESC",
                new SqlParameter("@userId", userId)
            ).ToList();

            return transactions;
        }

        public IEnumerable<dynamic> GetTransactionDetails(int transactionId)
        {
            // Using the vw_TransactionDetails view to get detailed information
            var transactionDetails = _context.Database.SqlQuery<dynamic>(
                "SELECT * FROM vw_TransactionDetails WHERE TransactionId = @transactionId",
                new SqlParameter("@transactionId", transactionId)
            ).ToList();

            return transactionDetails;
        }

        public IEnumerable<dynamic> GetUnfinishedOrders()
        {
            // Execute the stored procedure to get unfinished orders
            var result = _context.Database.SqlQuery<dynamic>(
                "EXEC sp_GetUnfinishedOrders"
            ).ToList();

            return result;
        }

        public IEnumerable<dynamic> GetSuccessfulTransactions()
        {
            // Execute the stored procedure to get successful transactions
            var result = _context.Database.SqlQuery<dynamic>(
                "EXEC sp_GetSuccessfulTransactions"
            ).ToList();

            return result;
        }

        public bool UpdateTransactionStatus(int transactionId, string newStatus)
        {
            try
            {
                // Execute the stored procedure to update transaction status
                var rowsAffectedParam = new SqlParameter
                {
                    ParameterName = "@RowsAffected",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlCommand(
                    "EXEC sp_UpdateTransactionStatus @TransactionId, @NewStatus, @RowsAffected OUTPUT",
                    new SqlParameter("@TransactionId", transactionId),
                    new SqlParameter("@NewStatus", newStatus),
                    rowsAffectedParam
                );

                // Check if any rows were affected
                if (rowsAffectedParam.Value != DBNull.Value && (int)rowsAffectedParam.Value > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateTransactionStatus: {ex.Message}");
                return false;
            }
        }

        public int CheckoutCart(int userId, int paymentMethodId)
        {
            try
            {
                // Debug info
                System.Diagnostics.Debug.WriteLine($"Repository CheckoutCart: userId={userId}, paymentMethodId={paymentMethodId}");

                // Execute the stored procedure to checkout the cart
                var transactionIdParam = new SqlParameter
                {
                    ParameterName = "@TransactionId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlCommand(
                    "EXEC sp_CheckoutCart @UserId, @PaymentMethodId, @TransactionId OUTPUT",
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@PaymentMethodId", paymentMethodId),
                    transactionIdParam
                );

                System.Diagnostics.Debug.WriteLine("Stored procedure executed");

                // Get the transaction ID from the output parameter
                if (transactionIdParam.Value != DBNull.Value)
                {
                    int transactionId = (int)transactionIdParam.Value;
                    System.Diagnostics.Debug.WriteLine($"Transaction ID: {transactionId}");
                    return transactionId;
                }

                System.Diagnostics.Debug.WriteLine("TransactionId parameter is null or DBNull");
                return -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CheckoutCart: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Adds a TransactionDetail to the database
        /// </summary>
        /// <param name="detail">TransactionDetail object to add</param>
        public void AddDetail(TransactionDetail detail)
        {
            try
            {
                _context.TransactionDetails.Add(detail);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in AddDetail: {ex.Message}");
                throw; // Re-throw to be handled by caller
            }
        }
        // Metode checkout tanpa menggunakan stored procedure
        public int DirectCheckout(int userId, int paymentMethodId)
        {
            try
            {
                // Gunakan transaction untuk memastikan semua operasi berhasil atau gagal bersama
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Buat header transaksi baru
                        var transactionHeader = new TransactionHeader
                        {
                            UserId = userId,
                            TransactionDate = DateTime.Now,
                            PaymentMethodId = paymentMethodId,
                            Status = "Payment Pending"
                        };

                        _context.TransactionHeaders.Add(transactionHeader);
                        _context.SaveChanges();

                        int transactionId = transactionHeader.TransactionId;
                        System.Diagnostics.Debug.WriteLine($"Created transaction header with ID: {transactionId}");

                        // 2. Ambil item-item di keranjang
                        var cartItems = _context.Carts.Where(c => c.UserId == userId).ToList();

                        if (!cartItems.Any())
                        {
                            transaction.Rollback();
                            System.Diagnostics.Debug.WriteLine("No items in cart, rollback transaction");
                            return -1;
                        }

                        System.Diagnostics.Debug.WriteLine($"Found {cartItems.Count} items in cart");

                        // 3. Buat detail transaksi untuk setiap item di keranjang
                        foreach (var cartItem in cartItems)
                        {
                            var transactionDetail = new TransactionDetail
                            {
                                TransactionId = transactionId,
                                JewelId = cartItem.JewelId,
                                Quantity = cartItem.Quantity
                            };

                            _context.TransactionDetails.Add(transactionDetail);
                        }

                        _context.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Added transaction details");

                        // 4. Hapus semua item di keranjang
                        foreach (var cartItem in cartItems)
                        {
                            _context.Carts.Remove(cartItem);
                        }

                        _context.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Removed all items from cart");

                        // 5. Commit transaction
                        transaction.Commit();
                        System.Diagnostics.Debug.WriteLine("Transaction committed successfully");

                        return transactionId;
                    }
                    catch (Exception ex)
                    {
                        // Rollback jika terjadi error
                        transaction.Rollback();
                        System.Diagnostics.Debug.WriteLine($"Error during transaction, rollback: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DirectCheckout: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return -1;
            }
        }
    }
}