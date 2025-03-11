<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="JAwelsAndDiamonds.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error - JAwels & Diamonds</title>
    <link href="~/Content/bootstrap.css" rel="stylesheet" />
    <link href="~/Assets/css/main.css" rel="stylesheet" />
    <style type="text/css">
        body {
            padding-top: 50px;
            padding-bottom: 20px;
            background-color: #f8f9fa;
        }
        .error-container {
            text-align: center;
            margin-top: 50px;
        }
        .error-code {
            font-size: 72px;
            margin-bottom: 0;
            color: #dc3545;
        }
        .error-message {
            font-size: 24px;
            margin-top: 0;
            margin-bottom: 20px;
        }
        .error-details {
            font-size: 16px;
            margin-bottom: 30px;
            color: #6c757d;
        }
        .home-button {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="error-container">
                <h1 class="error-code"><asp:Literal ID="ltErrorCode" runat="server">500</asp:Literal></h1>
                <h2 class="error-message"><asp:Literal ID="ltErrorMessage" runat="server">Internal Server Error</asp:Literal></h2>
                <p class="error-details">
                    <asp:Literal ID="ltErrorDetails" runat="server">
                        Sorry, something went wrong while processing your request.
                    </asp:Literal>
                </p>
                <div class="row">
                    <div class="col-md-6 offset-md-3">
                        <div class="alert alert-danger" runat="server" id="divErrorInfo" visible="false">
                            <strong>Error Information:</strong>
                            <asp:Literal ID="ltErrorInfo" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
                <div class="home-button">
                    <a href="~/Views/Home/Index.aspx" runat="server" class="btn btn-primary">Return to Home Page</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>