<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Profile.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AlertContent" runat="server">
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Literal ID="ltError" runat="server"></asp:Literal>
    </asp:Panel>
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="ltSuccess" runat="server"></asp:Literal>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h1 class="mb-4">Profile</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3>User Information</h3>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label">Email</label>
                        <p class="form-control-static"><asp:Literal ID="ltEmail" runat="server"></asp:Literal></p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Username</label>
                        <p class="form-control-static"><asp:Literal ID="ltUsername" runat="server"></asp:Literal></p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Gender</label>
                        <p class="form-control-static"><asp:Literal ID="ltGender" runat="server"></asp:Literal></p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Date of Birth</label>
                        <p class="form-control-static"><asp:Literal ID="ltDateOfBirth" runat="server"></asp:Literal></p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Role</label>
                        <p class="form-control-static"><asp:Literal ID="ltRole" runat="server"></asp:Literal></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3>Change Password</h3>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="txtOldPassword" class="form-label">Old Password</label>
                        <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter old password" required></asp:TextBox>
                        <small class="form-text text-muted">Must be the same as your current password.</small>
                    </div>
                    <div class="mb-3">
                        <label for="txtNewPassword" class="form-label">New Password</label>
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter new password" required></asp:TextBox>
                        <small class="form-text text-muted">Must be alphanumeric and 8 to 25 characters.</small>
                    </div>
                    <div class="mb-3">
                        <label for="txtConfirmPassword" class="form-label">Confirm Password</label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm new password" required></asp:TextBox>
                        <small class="form-text text-muted">Must be the same as new password.</small>
                    </div>
                    <div class="mb-3">
                        <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-primary" OnClick="btnChangePassword_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>