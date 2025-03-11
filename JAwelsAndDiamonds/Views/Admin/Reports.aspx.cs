using System;
using System.Data;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Admin
{
    public partial class Reports : System.Web.UI.Page
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
                // Load and display the Crystal Report
                LoadTransactionReport();
            }
        }

        private void LoadTransactionReport()
        {
            try
            {
                // Get successful transactions
                var transactions = _transactionController.GetSuccessfulTransactions();

                // Create DataSet for the report
                DataSet ds = CreateDataSetFromTransactions(transactions);

                // Load the Crystal Report
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(Server.MapPath("~/Reports/TransactionReport.rpt"));
                reportDocument.SetDataSource(ds);

                // Bind the report to the viewer
                crvTransactions.ReportSource = reportDocument;
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during report generation
                Response.Write($"<script>alert('Error loading report: {ex.Message}');</script>");
            }
        }

        private DataSet CreateDataSetFromTransactions(IEnumerable<dynamic> transactions)
        {
            // Create a new DataSet
            DataSet ds = new DataSet("TransactionDataSet");

            // Create transaction header table
            DataTable dtHeader = new DataTable("TransactionHeader");
            dtHeader.Columns.Add("TransactionId", typeof(int));
            dtHeader.Columns.Add("UserId", typeof(int));
            dtHeader.Columns.Add("Username", typeof(string));
            dtHeader.Columns.Add("TransactionDate", typeof(DateTime));
            dtHeader.Columns.Add("PaymentMethodName", typeof(string));
            dtHeader.Columns.Add("TotalAmount", typeof(decimal));

            // Create transaction detail table
            DataTable dtDetail = new DataTable("TransactionDetail");
            dtDetail.Columns.Add("TransactionDetailId", typeof(int));
            dtDetail.Columns.Add("TransactionId", typeof(int));
            dtDetail.Columns.Add("JewelId", typeof(int));
            dtDetail.Columns.Add("JewelName", typeof(string));
            dtDetail.Columns.Add("Price", typeof(decimal));
            dtDetail.Columns.Add("Quantity", typeof(int));
            dtDetail.Columns.Add("Subtotal", typeof(decimal));

            // Populate transaction header table
            decimal grandTotal = 0;
            foreach (var transaction in transactions)
            {
                DataRow headerRow = dtHeader.NewRow();
                headerRow["TransactionId"] = transaction.TransactionId;
                headerRow["UserId"] = transaction.UserId;
                headerRow["Username"] = transaction.Username;
                headerRow["TransactionDate"] = transaction.TransactionDate;
                headerRow["PaymentMethodName"] = transaction.PaymentMethodName;
                headerRow["TotalAmount"] = transaction.TotalAmount;

                // Add to grand total
                grandTotal += (decimal)transaction.TotalAmount;

                dtHeader.Rows.Add(headerRow);

                // Get transaction details
                var details = _transactionController.ViewTransactionDetail(transaction.TransactionId);

                // Populate transaction detail table
                foreach (var detail in details)
                {
                    DataRow detailRow = dtDetail.NewRow();
                    detailRow["TransactionDetailId"] = detail.TransactionDetailId;
                    detailRow["TransactionId"] = detail.TransactionId;
                    detailRow["JewelId"] = detail.JewelId;
                    detailRow["JewelName"] = detail.JewelName;
                    detailRow["Price"] = detail.Price;
                    detailRow["Quantity"] = detail.Quantity;
                    detailRow["Subtotal"] = detail.Subtotal;

                    dtDetail.Rows.Add(detailRow);
                }
            }

            // Add tables to DataSet
            ds.Tables.Add(dtHeader);
            ds.Tables.Add(dtDetail);

            // Create relation between header and detail
            DataRelation relation = new DataRelation(
                "TransactionDetails",
                ds.Tables["TransactionHeader"].Columns["TransactionId"],
                ds.Tables["TransactionDetail"].Columns["TransactionId"]
            );
            ds.Relations.Add(relation);

            // Create grand total table for report
            DataTable dtGrandTotal = new DataTable("GrandTotal");
            dtGrandTotal.Columns.Add("GrandTotalAmount", typeof(decimal));
            DataRow grandTotalRow = dtGrandTotal.NewRow();
            grandTotalRow["GrandTotalAmount"] = grandTotal;
            dtGrandTotal.Rows.Add(grandTotalRow);
            ds.Tables.Add(dtGrandTotal);

            return ds;
        }
    }
}