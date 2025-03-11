using System;
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

        protected void Page_Load(object sender, EventArgs e)
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

        private void LoadUnfinishedOrders()
        {
            // Get unfinished orders
            var orders = _transactionController.GetUnfinishedOrders();

            // Bind orders to grid view
            gvOrders.DataSource = orders;
            gvOrders.DataBind();

            // Show/hide panels based on order count
            bool hasOrders = gvOrders.Rows.Count > 0;
            pnlNoOrders.Visible = !hasOrders;
            pnlOrders.Visible = hasOrders;
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
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
    }
}