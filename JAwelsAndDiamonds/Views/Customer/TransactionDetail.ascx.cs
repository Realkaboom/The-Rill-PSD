using System;
using System.Linq;
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

        private void LoadTransactionDetails()
        {
            // Get transaction details
            var transactionDetails = _transactionController.ViewTransactionDetail(_transactionId).ToList();

            if (transactionDetails.Count == 0)
            {
                // Transaction not found, redirect to my orders page
                Response.Redirect("~/Views/Customer/MyOrders.aspx");
                return;
            }

            // Set transaction header information
            var firstDetail = transactionDetails.First();
            ltTransactionId.Text = firstDetail.TransactionId.ToString();
            ltTransactionDate.Text = Convert.ToDateTime(firstDetail.TransactionDate).ToString("MM/dd/yyyy");
            ltPaymentMethod.Text = firstDetail.PaymentMethodName;
            ltStatus.Text = firstDetail.Status;

            // Bind transaction details to grid view
            gvTransactionDetails.DataSource = transactionDetails;
            gvTransactionDetails.DataBind();

            // Calculate and display total
            decimal total = transactionDetails.Sum(d => Convert.ToDecimal(d.Subtotal));
            ltTotal.Text = string.Format("${0:0.00}", total);
        }
    }
}