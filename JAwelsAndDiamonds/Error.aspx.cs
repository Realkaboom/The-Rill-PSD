using System;
using System.Web;

namespace JAwelsAndDiamonds
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if there's a specific error code in the query string
                string errorCode = Request.QueryString["code"];
                switch (errorCode)
                {
                    case "404":
                        ltErrorCode.Text = "404";
                        ltErrorMessage.Text = "Page Not Found";
                        ltErrorDetails.Text = "The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.";
                        break;
                    case "403":
                        ltErrorCode.Text = "403";
                        ltErrorMessage.Text = "Access Denied";
                        ltErrorDetails.Text = "You do not have permission to access this resource.";
                        break;
                    default:
                        ltErrorCode.Text = "500";
                        ltErrorMessage.Text = "Internal Server Error";
                        ltErrorDetails.Text = "Sorry, something went wrong while processing your request.";
                        break;
                }

                // In development mode, show detailed error information
                if (HttpContext.Current.IsDebuggingEnabled)
                {
                    // Get the exception from server
                    Exception ex = Server.GetLastError();
                    if (ex != null)
                    {
                        divErrorInfo.Visible = true;
                        ltErrorInfo.Text = $"<div><strong>Exception:</strong> {HttpUtility.HtmlEncode(ex.Message)}</div>";

                        if (ex.InnerException != null)
                        {
                            ltErrorInfo.Text += $"<div><strong>Inner Exception:</strong> {HttpUtility.HtmlEncode(ex.InnerException.Message)}</div>";
                        }

                        ltErrorInfo.Text += $"<div><strong>Stack Trace:</strong> <pre>{HttpUtility.HtmlEncode(ex.StackTrace)}</pre></div>";

                        // Clear the error so it doesn't show again
                        Server.ClearError();
                    }
                }
            }
        }
    }
}