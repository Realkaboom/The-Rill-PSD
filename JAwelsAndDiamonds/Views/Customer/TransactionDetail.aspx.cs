using System;
using System.Linq;
using System.Collections.Generic;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Customer
{
    public partial class TransactionDetail : System.Web.UI.Page
    {
        private TransactionController _transactionController;
        private int _transactionId;

        // Model untuk detail transaksi
        public class TransactionDetailViewModel
        {
            public int TransactionDetailId { get; set; }
            public int JewelId { get; set; }
            public string JewelName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Subtotal { get; set; }
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

                // Get transaction ID from query string
                if (!int.TryParse(Request.QueryString["id"], out _transactionId) || _transactionId <= 0)
                {
                    // Invalid transaction ID, redirect to my orders page
                    Response.Redirect("~/Views/Customer/MyOrders.aspx");
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
                    // Load transaction details
                    LoadTransactionDetails();
                }
            }
            catch (Exception ex)
            {
                // Log error untuk debugging
                System.Diagnostics.Debug.WriteLine("Error in Page_Load: " + ex.ToString());
            }
        }

        private void LoadTransactionDetails()
        {
            try
            {
                // Get transaction details
                var transactionDetails = _transactionController.ViewTransactionDetail(_transactionId);

                if (transactionDetails == null || !transactionDetails.Any())
                {
                    // Transaction not found, redirect to my orders page
                    Response.Redirect("~/Views/Customer/MyOrders.aspx");
                    return;
                }

                // Convert to strongly-typed list for binding
                var detailsList = new List<TransactionDetailViewModel>();
                decimal total = 0;

                var firstDetail = transactionDetails.First();

                // Set transaction header information
                ltTransactionId.Text = firstDetail.TransactionId.ToString();
                ltTransactionDate.Text = Convert.ToDateTime(firstDetail.TransactionDate).ToString("MM/dd/yyyy");
                ltPaymentMethod.Text = firstDetail.PaymentMethodName != null ? firstDetail.PaymentMethodName.ToString() : "N/A";
                ltStatus.Text = firstDetail.Status != null ? firstDetail.Status.ToString() : "Unknown";

                foreach (var item in transactionDetails)
                {
                    try
                    {
                        var detail = new TransactionDetailViewModel
                        {
                            JewelId = Convert.ToInt32(item.JewelId),
                            JewelName = item.JewelName != null ? item.JewelName.ToString() : "Unknown",
                            Price = Convert.ToDecimal(item.Price),
                            Quantity = Convert.ToInt32(item.Quantity),
                            Subtotal = Convert.ToDecimal(item.Subtotal)
                        };

                        detailsList.Add(detail);
                        total += detail.Subtotal;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error converting detail: {ex.Message}");
                    }
                }

                // Bind transaction details to grid view
                gvTransactionDetails.DataSource = detailsList;
                gvTransactionDetails.DataBind();

                // Display total
                ltTotal.Text = string.Format("${0:0.00}", total);
            }
            catch (Exception ex)
            {
                // Log error untuk debugging
                System.Diagnostics.Debug.WriteLine("Error in LoadTransactionDetails: " + ex.ToString());
            }
        }
    }
}