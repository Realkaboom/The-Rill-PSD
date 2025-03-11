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
    public partial class Cart : System.Web.UI.Page
    {
        private CartController _cartController;
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

            // Initialize controllers
            JAwelsAndDiamondsEntities context = new JAwelsAndDiamondsEntities();

            // Cart controller
            ICartRepository cartRepository = new CartRepository(context);
            IJewelRepository jewelRepository = new JewelRepository(context);
            CartFactory cartFactory = new CartFactory();
            CartHandler cartHandler = new CartHandler(cartRepository, jewelRepository, cartFactory);
            _cartController = new CartController(cartHandler, this);

            // Transaction controller
            ITransactionRepository transactionRepository = new TransactionRepository(context);
            IPaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository(context);
            TransactionFactory transactionFactory = new TransactionFactory();
            TransactionHandler transactionHandler = new TransactionHandler(transactionRepository, cartRepository, paymentMethodRepository, transactionFactory);
            _transactionController = new TransactionController(transactionHandler, this);

            if (!IsPostBack)
            {
                // Load cart items
                LoadCart();

                // Load payment methods
                LoadPaymentMethods();
            }
        }

        private void LoadCart()
        {
            // Get cart items
            var cartItems = _cartController.ViewCart();

            // Bind cart items to grid view
            gvCartItems.DataSource = cartItems;
            gvCartItems.DataBind();

            // Calculate and display total
            decimal total = _cartController.GetCartTotal();
            lblTotal.Text = string.Format("${0:0.00}", total);

            // Show/hide panels based on cart status
            bool hasItems = gvCartItems.Rows.Count > 0;
            pnlEmptyCart.Visible = !hasItems;
            pnlCartItems.Visible = hasItems;
        }

        private void LoadPaymentMethods()
        {
            // Get payment methods
            var paymentMethods = _transactionController.GetAllPaymentMethods();

            // Clear existing items
            ddlPaymentMethod.Items.Clear();
            ddlPaymentMethod.Items.Add(new ListItem("-- Select Payment Method --", ""));

            // Add payment methods to dropdown
            foreach (var method in paymentMethods)
            {
                ddlPaymentMethod.Items.Add(new ListItem(method.PaymentMethodName, method.PaymentMethodId.ToString()));
            }
        }

        protected void gvCartItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int jewelId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "UpdateItem")
            {
                // Get the row index
                int rowIndex = GetRowIndexByJewelId(jewelId);
                if (rowIndex != -1)
                {
                    // Get the quantity textbox
                    TextBox txtQuantity = (TextBox)gvCartItems.Rows[rowIndex].FindControl("txtQuantity");

                    // Validate quantity
                    int quantity;
                    if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
                    {
                        // Show error message
                        pnlAlert.Visible = true;
                        ltError.Text = "Quantity must be a number greater than 0.";
                        return;
                    }

                    // Update the cart item
                    string errorMessage;
                    bool success = _cartController.UpdateCart(jewelId, quantity, out errorMessage);

                    if (!success)
                    {
                        // Show error message
                        pnlAlert.Visible = true;
                        ltError.Text = errorMessage;
                        return;
                    }

                    // Show success message
                    pnlSuccess.Visible = true;
                    ltSuccess.Text = "Cart item updated successfully.";

                    // Reload cart
                    LoadCart();
                }
            }
            else if (e.CommandName == "RemoveItem")
            {
                // Remove the cart item
                string errorMessage;
                bool success = _cartController.DeleteCartItem(jewelId, out errorMessage);

                if (!success)
                {
                    // Show error message
                    pnlAlert.Visible = true;
                    ltError.Text = errorMessage;
                    return;
                }

                // Show success message
                pnlSuccess.Visible = true;
                ltSuccess.Text = "Cart item removed successfully.";

                // Reload cart
                LoadCart();
            }
        }

        private int GetRowIndexByJewelId(int jewelId)
        {
            for (int i = 0; i < gvCartItems.Rows.Count; i++)
            {
                if (Convert.ToInt32(gvCartItems.DataKeys[i].Value) == jewelId)
                    return i;
            }
            return -1;
        }

        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            // Clear the cart
            string errorMessage;
            bool success = _cartController.ClearCart(out errorMessage);

            if (!success)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Show success message
            pnlSuccess.Visible = true;
            ltSuccess.Text = "Cart cleared successfully.";

            // Reload cart
            LoadCart();
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            // Validate payment method
            int paymentMethodId;
            if (!int.TryParse(ddlPaymentMethod.SelectedValue, out paymentMethodId) || paymentMethodId <= 0)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = "Please select a payment method.";
                return;
            }

            // Checkout the cart
            string errorMessage;
            int transactionId = _transactionController.Checkout(paymentMethodId, out errorMessage);

            if (transactionId == -1)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Redirect to orders page
            Response.Redirect("~/Views/Customer/MyOrders.aspx?checkout=true");
        }
    }
}