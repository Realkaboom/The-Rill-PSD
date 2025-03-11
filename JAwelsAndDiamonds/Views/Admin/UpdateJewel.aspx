<%@ Page Title="Update Jewel" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="UpdateJewel.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Admin.UpdateJewel" %>

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
            <h1 class="mb-4">Update Jewel</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h3>Jewel Information</h3>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="txtJewelName" class="form-label">Jewel Name</label>
                        <asp:TextBox ID="txtJewelName" runat="server" CssClass="form-control" placeholder="Enter jewel name" required></asp:TextBox>
                        <small class="form-text text-muted">Must be between 3 to 25 characters.</small>
                    </div>
                    <div class="mb-3">
                        <label for="ddlCategory" class="form-label">Category</label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" required>
                            <asp:ListItem Text="-- Select Category --" Value="" />
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label for="ddlBrand" class="form-label">Brand</label>
                        <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-select" required>
                            <asp:ListItem Text="-- Select Brand --" Value="" />
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label for="txtPrice" class="form-label">Price ($)</label>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Enter price" TextMode="Number" step="0.01" min="25.01" required></asp:TextBox>
                        <small class="form-text text-muted">Must be more than $25.</small>
                    </div>
                    <div class="mb-3">
                        <label for="txtReleaseYear" class="form-label">Release Year</label>
                        <asp:TextBox ID="txtReleaseYear" runat="server" CssClass="form-control" placeholder="Enter release year" TextMode="Number" required></asp:TextBox>
                        <small class="form-text text-muted">Must be less than the current year.</small>
                    </div>
                    <div class="mb-3">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                        <asp:Button ID="btnUpdateJewel" runat="server" Text="Update Jewel" CssClass="btn btn-primary" OnClick="btnUpdateJewel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>