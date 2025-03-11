using System;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Factories
{
    /// <summary>
    /// Factory for creating TransactionHeader and TransactionDetail objects
    /// </summary>
    public class TransactionFactory : IFactory<TransactionHeader>
    {
        /// <summary>
        /// Creates a new empty TransactionHeader instance
        /// </summary>
        /// <returns>A new TransactionHeader instance</returns>
        public TransactionHeader Create()
        {
            return new TransactionHeader();
        }

        /// <summary>
        /// Creates a new TransactionHeader with the specified properties
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="transactionDate">Transaction date</param>
        /// <param name="paymentMethodId">Payment method ID</param>
        /// <param name="status">Transaction status</param>
        /// <returns>A new TransactionHeader instance</returns>
        public TransactionHeader CreateTransactionHeader(int userId, DateTime transactionDate, int paymentMethodId, string status)
        {
            return new TransactionHeader
            {
                UserId = userId,
                TransactionDate = transactionDate,
                PaymentMethodId = paymentMethodId,
                Status = status
            };
        }

        /// <summary>
        /// Creates a new TransactionDetail with the specified properties
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="jewelId">Jewel ID</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>A new TransactionDetail instance</returns>
        public TransactionDetail CreateTransactionDetail(int transactionId, int jewelId, int quantity)
        {
            return new TransactionDetail
            {
                TransactionId = transactionId,
                JewelId = jewelId,
                Quantity = quantity
            };
        }
    }
}