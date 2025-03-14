using System;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Home
{
    public partial class JewelDetail : System.Web.UI.Page
    {
        private JewelController _jewelController;
        private CartController _cartController;
        private int _jewelId;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get jewel ID from query string
            if (!int.TryParse(Request.QueryString["id"], out _jewelId) || _jewelId <= 0)
            {
                // Invalid jewel ID, redirect to home page
                Response.Redirect("~/Views/Home/Index.aspx");
                return;
            }

            // Initialize the jewel controller
            JAwelsAndDiamondsEntities context = new JAwelsAndDiamondsEntities();
            IJewelRepository jewelRepository = new JewelRepository(context);
            ICategoryRepository categoryRepository = new CategoryRepository(context);
            IBrandRepository brandRepository = new BrandRepository(context);
            JewelFactory jewelFactory = new JewelFactory();
            JewelHandler jewelHandler = new JewelHandler(jewelRepository, categoryRepository, brandRepository, jewelFactory);
            _jewelController = new JewelController(jewelHandler, this);

            // Initialize the cart controller for customers
            ICartRepository cartRepository = new CartRepository(context);
            CartFactory cartFactory = new CartFactory();
            CartHandler cartHandler = new CartHandler(cartRepository, jewelRepository, cartFactory);
            _cartController = new CartController(cartHandler, this);

            if (!IsPostBack)
            {
                // Load jewel details
                LoadJewelDetails();

                // Set visibility of controls based on user role
                SetControlsVisibility();
            }
        }

        private void LoadJewelDetails()
        {
            // Get jewel details
            dynamic jewelDetails = _jewelController.GetJewelDetails(_jewelId);
            if (jewelDetails == null)
            {
                // Jewel not found, redirect to home page
                Response.Redirect("~/Views/Home/Index.aspx");
                return;
            }

            // Set jewel details
            ltJewelName.Text = jewelDetails.JewelName;
            ltCategory.Text = jewelDetails.CategoryName;
            ltBrand.Text = jewelDetails.BrandName;
            ltCountry.Text = jewelDetails.CountryOfOrigin;
            ltClass.Text = jewelDetails.Class;
            ltPrice.Text = string.Format("{0:0.00}", jewelDetails.Price);
            ltReleaseYear.Text = jewelDetails.ReleaseYear.ToString();
        }

        private void SetControlsVisibility()
        {
            // Get user role from session
            object role = SessionUtil.GetSession(Session, "Role");
            if (role == null)
            {
                // Guest - no special controls
                pnlCustomerControls.Visible = false;
                pnlAdminControls.Visible = false;
            }
            else if (role.ToString() == "Admin")
            {
                // Admin - show admin controls
                pnlCustomerControls.Visible = false;
                pnlAdminControls.Visible = true;
            }
            else if (role.ToString() == "Customer")
            {
                // Customer - show customer controls
                pnlCustomerControls.Visible = true;
                pnlAdminControls.Visible = false;
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            // Add the jewel to the cart with quantity 1
            string errorMessage;
            bool success = _cartController.AddToCart(_jewelId, 1, out errorMessage);

            if (!success)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Redirect to cart page
            Response.Redirect("~/Views/Customer/Cart.aspx");
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Redirect to edit jewel page
            Response.Redirect($"~/Views/Admin/UpdateJewel.aspx?id={_jewelId}");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete the jewel
            string errorMessage;
            bool success = _jewelController.DeleteJewel(_jewelId, out errorMessage);

            if (!success)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Redirect to home page with success message
            Response.Redirect("~/Views/Home/Index.aspx?deleted=true");
        }
    }
}