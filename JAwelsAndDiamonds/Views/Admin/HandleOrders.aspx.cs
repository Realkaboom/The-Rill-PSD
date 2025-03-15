using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Admin
{
    public partial class HandleOrders : System.Web.UI.Page
    {
        private TransactionController _transactionController;

        // Model untuk data transaksi
        public class OrderViewModel
        {
            public int TransactionId { get; set; }
            public int UserId { get; set; }
            public string Username { get; set; }
            public DateTime TransactionDate { get; set; }
            public string PaymentMethodName { get; set; }
            public string Status { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Check if user is logged in and is an admin
                object userId = SessionUtil.GetSession(Session, "UserId");
                object role = SessionUtil.GetSession(Session, "Role");

                if (userId == null || role == null || role.ToString() != "Admin")
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
                    // Load unfinished orders
                    LoadUnfinishedOrders();
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

        private void LoadUnfinishedOrders()
        {
            try
            {
                // Get unfinished orders
                var orders = _transactionController.GetUnfinishedOrders();

                // Debug log
                System.Diagnostics.Debug.WriteLine($"Raw orders data: {orders != null}");

                // Convert to strongly-typed list for binding
                var ordersList = new List<OrderViewModel>();

                if (orders != null)
                {
                    foreach (var item in orders)
                    {
                        try
                        {
                            // Konversi data dari dynamic object dengan penanganan null/error
                            var order = new OrderViewModel
                            {
                                TransactionId = Convert.ToInt32(item.TransactionId),
                                UserId = Convert.ToInt32(item.UserId),
                                Username = item.Username != null ? item.Username.ToString() : "Unknown",
                                TransactionDate = Convert.ToDateTime(item.TransactionDate),
                                PaymentMethodName = item.PaymentMethodName != null ? item.PaymentMethodName.ToString() : "N/A",
                                Status = item.Status != null ? item.Status.ToString() : "Unknown"
                            };

                            ordersList.Add(order);
                            System.Diagnostics.Debug.WriteLine($"Added order: ID={order.TransactionId}, Status={order.Status}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error converting order: {ex.Message}");
                        }
                    }
                }

                // Bind orders to grid view
                gvOrders.DataSource = ordersList;
                gvOrders.DataBind();

                // Show/hide panels based on order count
                bool hasOrders = gvOrders.Rows.Count > 0;
                pnlNoOrders.Visible = !hasOrders;
                pnlOrders.Visible = hasOrders;

                System.Diagnostics.Debug.WriteLine($"Total unfinished orders shown: {gvOrders.Rows.Count}");
            }
            catch (Exception ex)
            {
                // Tampilkan pesan error
                pnlAlert.Visible = true;
                ltError.Text = "Error loading orders: " + ex.Message;

                // Log error untuk debugging
                System.Diagnostics.Debug.WriteLine("Error in LoadUnfinishedOrders: " + ex.ToString());
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int transactionId = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "ConfirmPayment")
                {
                    // Confirm payment
                    string errorMessage;
                    bool success = _transactionController.ConfirmPayment(transactionId, out errorMessage);

                    if (!success)
                    {
                        // Show error message
                        pnlAlert.Visible = true;
                        ltError.Text = errorMessage;
                        return;
                    }

                    // Show success message
                    pnlSuccess.Visible = true;
                    ltSuccess.Text = "Payment confirmed successfully.";

                    // Reload orders
                    LoadUnfinishedOrders();
                }
                else if (e.CommandName == "ShipPackage")
                {
                    // Ship package
                    string errorMessage;
                    bool success = _transactionController.ShipPackage(transactionId, out errorMessage);

                    if (!success)
                    {
                        // Show error message
                        pnlAlert.Visible = true;
                        ltError.Text = errorMessage;
                        return;
                    }

                    // Show success message
                    pnlSuccess.Visible = true;
                    ltSuccess.Text = "Package shipped successfully.";

                    // Reload orders
                    LoadUnfinishedOrders();
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