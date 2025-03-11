<%@ Page Title="Register" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Auth.Register" %>

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
                    <h3 class="card-title">Register</h3>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter your email" required></asp:TextBox>
                        <small class="form-text text-muted">Must be a valid email format.</small>
                    </div>
                    <div class="mb-3">
                        <label for="txtUsername" class="form-label">Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username" required></asp:TextBox>
                        <small class="form-text text-muted">Must be between 3 to 25 characters.</small>
                    </div>
                    <div class="mb-3">
                        <label for="txtPassword" class="form-label">Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter your password" required></asp:TextBox>
                        <small class="form-text text-muted">Must be alphanumeric and 8 to 20 characters.</small>
                    </div>
                    <div class="mb-3">
                        <label for="txtConfirmPassword" class="form-label">Confirm Password</label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirm your password" required></asp:TextBox>
                        <small class="form-text text-muted">Must be the same as password.</small>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Gender</label>
                        <div class="form-check">
                            <asp:RadioButton ID="rbMale" runat="server" GroupName="Gender" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label" for="rbMale">Male</label>
                        </div>
                        <div class="form-check">
                            <asp:RadioButton ID="rbFemale" runat="server" GroupName="Gender" CssClass="form-check-input" />
                            <label class="form-check-label" for="rbFemale">Female</label>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label for="dpDateOfBirth" class="form-label">Date of Birth</label>
                        <asp:TextBox ID="dpDateOfBirth" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                        <small class="form-text text-muted">Must be earlier than 01/01/2010.</small>
                    </div>
                    <div class="mb-3">
                        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary" OnClick="btnRegister_Click" />
                    </div>
                    <div class="mb-3">
                        <p>Already have an account? <a href="Login.aspx">Login here</a></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="~/Assets/js/validation.js"></script>
</asp:Content>