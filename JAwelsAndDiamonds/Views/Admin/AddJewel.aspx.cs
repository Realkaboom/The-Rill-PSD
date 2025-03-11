using System;
using System.Linq;
using System.Web.UI.WebControls;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Admin
{
    public partial class AddJewel : System.Web.UI.Page
    {
        private JewelController _jewelController;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Periksa apakah user adalah admin
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Views/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Inisialisasi controller dan load data
                InitializeController();
                LoadCategories();
                LoadBrands();
            }
        }

        private void InitializeController()
        {
            JAwelsAndDiamondsEntities context = new JAwelsAndDiamondsEntities();
            IJewelRepository jewelRepository = new JewelRepository(context);
            ICategoryRepository categoryRepository = new CategoryRepository(context);
            IBrandRepository brandRepository = new BrandRepository(context);
            JewelFactory jewelFactory = new JewelFactory();
            JewelHandler jewelHandler = new JewelHandler(jewelRepository, categoryRepository, brandRepository, jewelFactory);
            _jewelController = new JewelController(jewelHandler, this);
        }

        private void LoadCategories()
        {
            try
            {
                // Get all categories menggunakan JewelController
                var categories = _jewelController.GetAllCategories();

                // Bind ke dropdown
                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryId";
                ddlCategory.DataBind();

                // Tambahkan item default jika belum ada
                if (ddlCategory.Items.FindByValue("") == null)
                {
                    ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
                }
            }
            catch (Exception ex)
            {
                // Tampilkan pesan error
                pnlAlert.Visible = true;
                ltError.Text = $"Error loading categories: {ex.Message}";
            }
        }

        private void LoadBrands()
        {
            try
            {
                // Get all brands menggunakan JewelController
                var brands = _jewelController.GetAllBrands();

                // Bind ke dropdown
                ddlBrand.DataSource = brands;
                ddlBrand.DataTextField = "BrandName";
                ddlBrand.DataValueField = "BrandId";
                ddlBrand.DataBind();

                // Tambahkan item default jika belum ada
                if (ddlBrand.Items.FindByValue("") == null)
                {
                    ddlBrand.Items.Insert(0, new ListItem("-- Select Brand --", ""));
                }
            }
            catch (Exception ex)
            {
                // Tampilkan pesan error
                pnlAlert.Visible = true;
                ltError.Text = $"Error loading brands: {ex.Message}";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect to home page
            Response.Redirect("~/Views/Home/Index.aspx");
        }

        protected void btnAddJewel_Click(object sender, EventArgs e)
        {
            if (_jewelController == null)
            {
                InitializeController();
            }

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

            // Add the jewel
            string errorMessage;
            bool success = _jewelController.AddJewel(jewelName, price, releaseYear, categoryId, brandId, out errorMessage);

            if (!success)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Clear form
            txtJewelName.Text = "";
            ddlCategory.SelectedIndex = 0;
            ddlBrand.SelectedIndex = 0;
            txtPrice.Text = "";
            txtReleaseYear.Text = "";

            // Show success message
            pnlSuccess.Visible = true;
            ltSuccess.Text = "Jewel added successfully.";

            ltSuccess.Text += " <a href='~/Views/Home/Index.aspx' runat='server'>Return to Home</a>";
        }
    }
}