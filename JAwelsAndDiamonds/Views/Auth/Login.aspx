<%@ Page Title="Login" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Auth.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AlertContent" runat="server">
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Literal ID="ltError" runat="server"></asp:Literal>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">Login</h3>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter your email" required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtPassword" class="form-label">Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter your password" required></asp:TextBox>
                    </div>
                    <div class="mb-3 form-check">
                        <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label" for="chkRememberMe">Remember me</label>
                    </div>
                    <div class="mb-3">
                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                    </div>
                    <div class="mb-3">
                        <p>Don't have an account? <a href="Register.aspx">Register here</a></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="~/Assets/js/validation.js"></script>
</asp:Content>