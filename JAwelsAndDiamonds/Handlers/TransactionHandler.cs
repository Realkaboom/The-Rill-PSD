using System;
using System.Collections.Generic;
using System.Linq;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;

namespace JAwelsAndDiamonds.Handlers
{
    /// <summary>
    /// Handler for transaction-related business logic
    /// </summary>
    public class TransactionHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly TransactionFactory _transactionFactory;

        /// <summary>
        /// Constructor for TransactionHandler
        /// </summary>
        /// <param name="transactionRepository">Transaction repository</param>
        /// <param name="cartRepository">Cart repository</param>
        /// <param name="paymentMethodRepository">Payment method repository</param>
        /// <param name="transactionFactory">Transaction factory</param>
        public TransactionHandler(
        ITransactionRepository transactionRepository,
        ICartRepository cartRepository,
        IPaymentMethodRepository paymentMethodRepository,
        TransactionFactory transactionFactory)
        {
            _transactionRepository = transactionRepository;
            _cartRepository = cartRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Checks out a user's cart and creates a transaction
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="paymentMethodId">Payment method ID</param>
        /// <returns>The transaction ID if successful, otherwise -1</returns>
        public int CheckoutCart(int userId, int paymentMethodId)
        {
            try
            {
                // Check if the payment method exists
                if (_paymentMethodRepository.GetById(paymentMethodId) == null)
                {
                    System.Diagnostics.Debug.WriteLine($"CheckoutCart: Payment method {paymentMethodId} not found");
                    return -1;
                }

                // Debug info
                System.Diagnostics.Debug.WriteLine($"CheckoutCart: Processing for userId={userId}, paymentMethodId={paymentMethodId}");

                // Periksa secara langsung di database jika cart memiliki item
                bool hasItems = _cartRepository.HasItems(userId);
                System.Diagnostics.Debug.WriteLine($"CheckoutCart: HasItems={hasItems}");

                if (!hasItems)
                {
                    System.Diagnostics.Debug.WriteLine("CheckoutCart: Cart is empty");
                    return -1;
                }

                // Use the repository's method to checkout the cart
                return _transactionRepository.CheckoutCart(userId, paymentMethodId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CheckoutCart Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Gets all transactions for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>IEnumerable of transactions for the specified user</returns>
        public IEnumerable<dynamic> GetUserTransactions(int userId)
        {
            return _transactionRepository.GetTransactionsByUserId(userId);
        }

        /// <summary>
        /// Gets transaction details by transaction ID
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>IEnumerable of transaction details</returns>
        public IEnumerable<dynamic> GetTransactionDetails(int transactionId)
        {
            return _transactionRepository.GetTransactionDetails(transactionId);
        }

        /// <summary>
        /// Updates the status of a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="newStatus">New status</param>
        /// <returns>True if the update was successful, otherwise false</returns>
        public bool UpdateTransactionStatus(int transactionId, string newStatus)
        {
            // Validate the new status
            if (newStatus != "Payment Pending" &&
                newStatus != "Shipment Pending" &&
                newStatus != "Arrived" &&
                newStatus != "Done" &&
                newStatus != "Rejected")
                return false;

            return _transactionRepository.UpdateTransactionStatus(transactionId, newStatus);
        }

        /// <summary>
        /// Gets unfinished orders (orders not in "Done" or "Rejected" status)
        /// </summary>
        /// <returns>IEnumerable of unfinished orders</returns>
        public IEnumerable<dynamic> GetUnfinishedOrders()
        {
            return _transactionRepository.GetUnfinishedOrders();
        }

        /// <summary>
        /// Gets successful transactions (transactions with "Done" status)
        /// </summary>
        /// <returns>IEnumerable of successful transactions</returns>
        public IEnumerable<dynamic> GetSuccessfulTransactions()
        {
            return _transactionRepository.GetSuccessfulTransactions();
        }

        /// <summary>
        /// Confirms payment for a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>True if the confirmation was successful, otherwise false</returns>
        public bool ConfirmPayment(int transactionId)
        {
            return UpdateTransactionStatus(transactionId, "Shipment Pending");
        }

        /// <summary>
        /// Ships a package for a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>True if the shipping was successful, otherwise false</returns>
        public bool ShipPackage(int transactionId)
        {
            return UpdateTransactionStatus(transactionId, "Arrived");
        }

        /// <summary>
        /// Confirms a package for a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>True if the confirmation was successful, otherwise false</returns>
        public bool ConfirmPackage(int transactionId)
        {
            return UpdateTransactionStatus(transactionId, "Done");
        }

        /// <summary>
        /// Rejects a package for a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>True if the rejection was successful, otherwise false</returns>
        public bool RejectPackage(int transactionId)
        {
            return UpdateTransactionStatus(transactionId, "Rejected");
        }

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <returns>IEnumerable of all payment methods</returns>
        public IEnumerable<PaymentMethod> GetAllPaymentMethods()
        {
            return _paymentMethodRepository.GetAll();
        }
    }
}