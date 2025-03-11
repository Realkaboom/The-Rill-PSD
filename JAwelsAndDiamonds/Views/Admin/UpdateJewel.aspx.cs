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
    public partial class UpdateJewel : System.Web.UI.Page
    {
        private JewelController _jewelController;
        private int _jewelId;

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

            // Get jewel ID from query string
            if (!int.TryParse(Request.QueryString["id"], out _jewelId) || _jewelId <= 0)
            {
                // Invalid jewel ID, redirect to home page
                Response.Redirect("~/Views/Home/Index.aspx");
                return;
            }

            // Initialize jewel controller
            JAwelsAndDiamondsEntities context = new JAwelsAndDiamondsEntities();
            IJewelRepository jewelRepository = new JewelRepository(context);
            ICategoryRepository categoryRepository = new CategoryRepository(context);
            IBrandRepository brandRepository = new BrandRepository(context);
            JewelFactory jewelFactory = new JewelFactory();
            JewelHandler jewelHandler = new JewelHandler(jewelRepository, categoryRepository, brandRepository, jewelFactory);
            _jewelController = new JewelController(jewelHandler, this);

            if (!IsPostBack)
            {
                // Load categories and brands
                LoadCategories();
                LoadBrands();

                // Set maximum release year
                txtReleaseYear.Attributes["max"] = DateTime.Now.Year.ToString();

                // Load jewel details
                LoadJewelDetails();
            }
        }

        private void LoadCategories()
        {
            // Get all categories
            var categories = _jewelController.GetAllCategories();

            // Clear existing items
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("-- Select Category --", ""));

            // Add categories to dropdown
            foreach (var category in categories)
            {
                ddlCategory.Items.Add(new ListItem(category.CategoryName, category.CategoryId.ToString()));
            }
        }

        private void LoadBrands()
        {
            // Get all brands
            var brands = _jewelController.GetAllBrands();

            // Clear existing items
            ddlBrand.Items.Clear();
            ddlBrand.Items.Add(new ListItem("-- Select Brand --", ""));

            // Add brands to dropdown
            foreach (var brand in brands)
            {
                ddlBrand.Items.Add(new ListItem(brand.BrandName, brand.BrandId.ToString()));
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

            // Set form values
            txtJewelName.Text = jewelDetails.JewelName;

            // Set selected category
            ListItem categoryItem = ddlCategory.Items.FindByValue(jewelDetails.CategoryId.ToString());
            if (categoryItem != null)
                categoryItem.Selected = true;

            // Set selected brand
            ListItem brandItem = ddlBrand.Items.FindByValue(jewelDetails.BrandId.ToString());
            if (brandItem != null)
                brandItem.Selected = true;

            txtPrice.Text = jewelDetails.Price.ToString("0.00");
            txtReleaseYear.Text = jewelDetails.ReleaseYear.ToString();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect back to jewel detail page
            Response.Redirect($"~/Views/Home/JewelDetail.aspx?id={_jewelId}");
        }

        protected void btnUpdateJewel_Click(object sender, EventArgs e)
        {
            // Get form values
            string jewelName = txtJewelName.Text.Trim();

            int categoryId;
            if (!int.TryParse(ddlCategory.SelectedValue, out categoryId) || categoryId <= 0)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = "Please select a category.";
                return;
            }

            int brandId;
            if (!int.TryParse(ddlBrand.SelectedValue, out brandId) || brandId <= 0)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = "Please select a brand.";
                return;
            }

            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 25)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = "Price must be a number more than $25.";
                return;
            }

            int releaseYear;
            if (!int.TryParse(txtReleaseYear.Text, out releaseYear) || releaseYear > DateTime.Now.Year)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = "Release year must be a number less than the current year.";
                return;
            }

            // Update the jewel
            string errorMessage;
            bool success = _jewelController.UpdateJewel(_jewelId, jewelName, price, releaseYear, categoryId, brandId, out errorMessage);

            if (!success)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Show success message
            pnlSuccess.Visible = true;
            ltSuccess.Text = "Jewel updated successfully.";

            // Reload jewel details after update
            LoadJewelDetails();
        }
    }
}