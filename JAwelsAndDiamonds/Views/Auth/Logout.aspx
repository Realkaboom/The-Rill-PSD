<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Auth.Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logging Out</title>
    <meta http-equiv="refresh" content="2;url=../Home/Index.aspx" />
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container text-center mt-5">
            <h2>Logging Out...</h2>
            <p>Please wait while we log you out.</p>
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-3">Redirecting to home page...</p>
        </div>
    </form>
</body>
</html>