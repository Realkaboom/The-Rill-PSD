using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Controllers
{
    /// <summary>
    /// Controller for transaction-related operations
    /// </summary>
    public class TransactionController
    {
        private readonly TransactionHandler _transactionHandler;
        private readonly Page _page;

        /// <summary>
        /// Constructor for TransactionController
        /// </summary>
        /// <param name="transactionHandler">Transaction handler</param>
        /// <param name="page">The current page</param>
        public TransactionController(TransactionHandler transactionHandler, Page page)
        {
            _transactionHandler = transactionHandler;
            _page = page;
        }

        /// <summary>
        /// Gets all transactions for the current user
        /// </summary>
        /// <returns>IEnumerable of transactions for the current user</returns>
        public IEnumerable<dynamic> ViewUserTransactions()
        {
            // Get the current user ID from session
            int userId = (int)SessionUtil.GetSession(_page.Session, "UserId");
            return _transactionHandler.GetUserTransactions(userId);
        }

        /// <summary>
        /// Gets transaction details by transaction ID
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>IEnumerable of transaction details</returns>
        public IEnumerable<dynamic> ViewTransactionDetail(int transactionId)
        {
            return _transactionHandler.GetTransactionDetails(transactionId);
        }

        /// <summary>
        /// Confirms a package for a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the confirmation was successful, otherwise false</returns>
        public bool ConfirmPackage(int transactionId, out string errorMessage)
        {
            errorMessage = "";

            // Confirm the package
            bool success = _transactionHandler.ConfirmPackage(transactionId);
            if (!success)
            {
                errorMessage = "Failed to confirm the package.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Rejects a package for a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the rejection was successful, otherwise false</returns>
        public bool RejectPackage(int transactionId, out string errorMessage)
        {
            errorMessage = "";

            // Reject the package
            bool success = _transactionHandler.RejectPackage(transactionId);
            if (!success)
            {
                errorMessage = "Failed to reject the package.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets unfinished orders (orders not in "Done" or "Rejected" status)
        /// </summary>
        /// <returns>IEnumerable of unfinished orders</returns>
        public IEnumerable<dynamic> GetUnfinishedOrders()
        {
            return _transactionHandler.GetUnfinishedOrders();
        }

        /// <summary>
        /// Confirms payment for a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the confirmation was successful, otherwise false</returns>
        public bool ConfirmPayment(int transactionId, out string errorMessage)
        {
            errorMessage = "";

            // Confirm the payment
            bool success = _transactionHandler.ConfirmPayment(transactionId);
            if (!success)
            {
                errorMessage = "Failed to confirm the payment.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ships a package for a transaction
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the shipping was successful, otherwise false</returns>
        public bool ShipPackage(int transactionId, out string errorMessage)
        {
            errorMessage = "";

            // Ship the package
            bool success = _transactionHandler.ShipPackage(transactionId);
            if (!success)
            {
                errorMessage = "Failed to ship the package.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets successful transactions (transactions with "Done" status)
        /// </summary>
        /// <returns>IEnumerable of successful transactions</returns>
        public IEnumerable<dynamic> GetSuccessfulTransactions()
        {
            return _transactionHandler.GetSuccessfulTransactions();
        }

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <returns>IEnumerable of all payment methods</returns>
        public IEnumerable<PaymentMethod> GetAllPaymentMethods()
        {
            return _transactionHandler.GetAllPaymentMethods();
        }

        /// <summary>
        /// Checks out the current user's cart
        /// </summary>
        /// <param name="paymentMethodId">Payment method ID</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>The transaction ID if successful, otherwise -1</returns>
        public int Checkout(int paymentMethodId, out string errorMessage)
        {
            errorMessage = "";

            try
            {
                // Validate payment method ID
                if (paymentMethodId <= 0)
                {
                    errorMessage = "Please select a payment method.";
                    return -1;
                }

                // Get the current user ID from session
                int userId = (int)SessionUtil.GetSession(_page.Session, "UserId");

                // Debug log
                System.Diagnostics.Debug.WriteLine($"Attempting checkout: UserId={userId}, PaymentMethodId={paymentMethodId}");

                // Bypass HasItems check temporarily:
                // Direct database check to make sure we have items
                var context = new JAwelsAndDiamondsEntities();
                bool hasItems = context.Carts.Any(c => c.UserId == userId);

                if (!hasItems)
                {
                    errorMessage = "Failed to checkout the cart. Make sure the cart is not empty.";
                    System.Diagnostics.Debug.WriteLine("Checkout failed: Cart is empty");
                    return -1;
                }

                // Checkout the cart
                int transactionId = _transactionHandler.CheckoutCart(userId, paymentMethodId);

                if (transactionId == -1)
                {
                    errorMessage = "Failed to checkout the cart. Technical issue occurred.";
                    System.Diagnostics.Debug.WriteLine("Checkout failed: Transaction ID is -1");
                    return -1;
                }

                System.Diagnostics.Debug.WriteLine($"Checkout successful: TransactionId={transactionId}");
                return transactionId;
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred during checkout: " + ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception during checkout: " + ex.ToString());
                return -1;
            }
        }
    }
}