using System;
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

        protected void Page_Load(object sender, EventArgs e)
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

        private void LoadOrders()
        {
            // Get user's transactions
            var transactions = _transactionController.ViewUserTransactions();

            // Bind transactions to grid view
            gvOrders.DataSource = transactions;
            gvOrders.DataBind();

            // Show/hide panels based on order count
            bool hasOrders = gvOrders.Rows.Count > 0;
            pnlNoOrders.Visible = !hasOrders;
            pnlOrders.Visible = hasOrders;
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
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
    }
}