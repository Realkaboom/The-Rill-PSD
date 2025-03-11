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
            catch
            {
                return false;
            }
        }

        public int CheckoutCart(int userId, int paymentMethodId)
        {
            try
            {
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

                // Get the transaction ID from the output parameter
                if (transactionIdParam.Value != DBNull.Value)
                {
                    return (int)transactionIdParam.Value;
                }

                return -1;
            }
            catch
            {
                return -1;
            }
        }
    }
}