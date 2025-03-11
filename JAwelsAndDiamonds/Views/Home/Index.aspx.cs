using System;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;

namespace JAwelsAndDiamonds.Views.Home
{
    public partial class Index : System.Web.UI.Page
    {
        private JewelController _jewelController;

        protected void Page_Load(object sender, EventArgs e)
        {
            JAwelsAndDiamondsEntities context = new JAwelsAndDiamondsEntities();
            IJewelRepository jewelRepository = new JewelRepository(context);
            ICategoryRepository categoryRepository = new CategoryRepository(context);
            IBrandRepository brandRepository = new BrandRepository(context);
            JewelFactory jewelFactory = new JewelFactory();
            JewelHandler jewelHandler = new JewelHandler(jewelRepository, categoryRepository, brandRepository, jewelFactory);
            _jewelController = new JewelController(jewelHandler, this);

            if (!IsPostBack)
            {
                LoadJewels();
            }
        }

        private void LoadJewels()
        {
            var jewels = _jewelController.GetAllJewels();
            rptJewels.DataSource = jewels;
            rptJewels.DataBind();
        }
    }
}