using System.Collections.Generic;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public interface ITransactionRepository : IRepository<TransactionHeader>
    {
        /// <summary>
        /// Gets transactions by user ID
        /// </summary>
        /// <param name="userId">User ID to filter by</param>
        /// <returns>IEnumerable of transactions for the specified user</returns>
        IEnumerable<dynamic> GetTransactionsByUserId(int userId);

        /// <summary>
        /// Gets transaction details by transaction ID
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>IEnumerable of transaction details</returns>
        IEnumerable<dynamic> GetTransactionDetails(int transactionId);

        /// <summary>
        /// Gets unfinished orders (orders not in "Done" or "Rejected" status)
        /// </summary>
        /// <returns>IEnumerable of unfinished orders</returns>
        IEnumerable<dynamic> GetUnfinishedOrders();

        /// <summary>
        /// Gets successful transactions (transactions with "Done" status)
        /// </summary>
        /// <returns>IEnumerable of successful transactions</returns>
        IEnumerable<dynamic> GetSuccessfulTransactions();

        /// <summary>
        /// Updates transaction status
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="newStatus">New status</param>
        /// <returns>True if successful, otherwise false</returns>
        bool UpdateTransactionStatus(int transactionId, string newStatus);

        /// <summary>
        /// Creates a new transaction from cart items
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="paymentMethodId">Payment method ID</param>
        /// <returns>The transaction ID if successful, otherwise -1</returns>
        int CheckoutCart(int userId, int paymentMethodId);

        /// <summary>
        /// Adds a TransactionDetail to the database
        /// </summary>
        /// <param name="detail">TransactionDetail object to add</param>
        void AddDetail(TransactionDetail detail);
    }
}