<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_GuestNavigation.ascx.cs" Inherits="JAwelsAndDiamonds.Views.Shared._GuestNavigation" %>

<nav class="navbar navbar-expand-lg navbar-light bg-light">
    <div class="container-fluid">
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link" href="~/Views/Home/Index.aspx" runat="server">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="~/Views/Auth/Login.aspx" runat="server">Login</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="~/Views/Auth/Register.aspx" runat="server">Register</a>
                </li>
            </ul>
        </div>
    </div>
</nav>