using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Customer
{
    public partial class MyOrders : System.Web.UI.Page
    {
        private TransactionController _transactionController;

        // Model untuk data transaksi
        public class TransactionViewModel
        {
            public int TransactionId { get; set; }
            public DateTime TransactionDate { get; set; }
            public string PaymentMethodName { get; set; }
            public string Status { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Check if user is logged in and is a customer
                object userId = SessionUtil.GetSession(Session, "UserId");
                object role = SessionUtil.GetSession(Session, "Role");

                if (userId == null || role == null || role.ToString() != "Customer")
                {
                    // Redirect to login page
                    Response.Redirect("~/Views/Auth/Login.aspx");
                    return;
                }

                // Initialize transaction controller
                JAwelsAndDiamondsEntities context = new JAwelsAndDiamondsEntities();
                ITransactionRepository transactionRepository = new TransactionRepository(context);
                ICartRepository cartRepository = new CartRepository(context);
                IPaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository(context);
                TransactionFactory transactionFactory = new TransactionFactory();
                TransactionHandler transactionHandler = new TransactionHandler(transactionRepository, cartRepository, paymentMethodRepository, transactionFactory);
                _transactionController = new TransactionController(transactionHandler, this);

                if (!IsPostBack)
                {
                    // Check for checkout success message
                    if (Request.QueryString["checkout"] == "true")
                    {
                        pnlSuccess.Visible = true;
                        ltSuccess.Text = "Order placed successfully.";
                    }

                    // Load orders
                    LoadOrders();
                }
            }
            catch (Exception ex)
            {
                // Tampilkan pesan error
                pnlAlert.Visible = true;
                ltError.Text = "Error initializing page: " + ex.Message;

                // Log error untuk debugging
                System.Diagnostics.Debug.WriteLine("Error in Page_Load: " + ex.ToString());
            }
        }

        private void LoadOrders()
        {
            try
            {
                // Get user's transactions
                var transactions = _transactionController.ViewUserTransactions();

                // Debug log
                System.Diagnostics.Debug.WriteLine($"Raw transactions data: {transactions != null}");

                // Konversi ke list model yang cocok untuk binding
                var transactionList = new List<TransactionViewModel>();

                if (transactions != null)
                {
                    foreach (var item in transactions)
                    {
                        try
                        {
                            // Konversi data dari dynamic object dengan penanganan null/error
                            var transaction = new TransactionViewModel
                            {
                                TransactionId = Convert.ToInt32(item.TransactionId),
                                TransactionDate = Convert.ToDateTime(item.TransactionDate),
                                PaymentMethodName = item.PaymentMethodName != null ? item.PaymentMethodName.ToString() : "N/A",
                                Status = item.Status != null ? item.Status.ToString() : "Unknown"
                            };

                            transactionList.Add(transaction);
                            System.Diagnostics.Debug.WriteLine($"Added transaction: ID={transaction.TransactionId}, Status={transaction.Status}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error converting transaction: {ex.Message}");
                        }
                    }
                }

                // Bind transactions to grid view
                gvOrders.DataSource = transactionList;
                gvOrders.DataBind();

                // Show/hide panels based on order count
                bool hasOrders = gvOrders.Rows.Count > 0;
                pnlNoOrders.Visible = !hasOrders;
                pnlOrders.Visible = hasOrders;

                System.Diagnostics.Debug.WriteLine($"Total orders shown: {gvOrders.Rows.Count}");
            }
            catch (Exception ex)
            {
                // Tampilkan pesan error
                pnlAlert.Visible = true;
                ltError.Text = "Error loading orders: " + ex.Message;

                // Log error untuk debugging
                System.Diagnostics.Debug.WriteLine("Error in LoadOrders: " + ex.ToString());
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int transactionId = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "ViewDetails")
                {
                    // Redirect to transaction detail page
                    Response.Redirect($"~/Views/Customer/TransactionDetail.aspx?id={transactionId}");
                }
                else if (e.CommandName == "ConfirmPackage")
                {
                    // Confirm the package
                    string errorMessage;
                    bool success = _transactionController.ConfirmPackage(transactionId, out errorMessage);

                    if (!success)
                    {
                        // Show error message
                        pnlAlert.Visible = true;
                        ltError.Text = errorMessage;
                        return;
                    }

                    // Show success message
                    pnlSuccess.Visible = true;
                    ltSuccess.Text = "Package confirmed successfully.";

                    // Reload orders
                    LoadOrders();
                }
                else if (e.CommandName == "RejectPackage")
                {
                    // Reject the package
                    string errorMessage;
                    bool success = _transactionController.RejectPackage(transactionId, out errorMessage);

                    if (!success)
                    {
                        // Show error message
                        pnlAlert.Visible = true;
                        ltError.Text = errorMessage;
                        return;
                    }

                    // Show success message
                    pnlSuccess.Visible = true;
                    ltSuccess.Text = "Package rejected successfully.";

                    // Reload orders
                    LoadOrders();
                }
            }
            catch (Exception ex)
            {
                // Tampilkan pesan error
                pnlAlert.Visible = true;
                ltError.Text = "Error processing command: " + ex.Message;

                // Log error untuk debugging
                System.Diagnostics.Debug.WriteLine("Error in gvOrders_RowCommand: " + ex.ToString());
            }
        }
    }
}