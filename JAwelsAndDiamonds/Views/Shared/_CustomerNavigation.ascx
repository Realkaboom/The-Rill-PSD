<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_CustomerNavigation.ascx.cs" Inherits="JAwelsAndDiamonds.Views.Shared._CustomerNavigation" %>

<nav class="navbar navbar-expand-lg navbar-light bg-light">
    <div class="container-fluid">
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav me-auto">
                <li class="nav-item">
                    <a class="nav-link" href="/Views/Home/Index.aspx">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/Views/Customer/Cart.aspx">Cart</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/Views/Customer/MyOrders.aspx">My Orders</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/Views/Profile/Profile.aspx">Profile</a>
                </li>
            </ul>
            <ul class="navbar-nav">
                <li class="nav-item">
                    <span class="nav-link">Welcome, <asp:Literal ID="ltUsername" runat="server"></asp:Literal></span>
                </li>
                <li class="nav-item">
                    <a href="~/Views/Auth/Logout.aspx" runat="server" class="nav-link">Logout</a>
                </li>
            </ul>
        </div>
    </div>
</nav>