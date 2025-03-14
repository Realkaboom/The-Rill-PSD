<%@ Page Title="Jewel Detail" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="JewelDetail.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Home.JewelDetail" %>

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
            <h1 class="mb-4">Jewel Details</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3><asp:Literal ID="ltJewelName" runat="server"></asp:Literal></h3>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <p><strong>Category:</strong> <asp:Literal ID="ltCategory" runat="server"></asp:Literal></p>
                            <p><strong>Brand:</strong> <asp:Literal ID="ltBrand" runat="server"></asp:Literal></p>
                            <p><strong>Country of Origin:</strong> <asp:Literal ID="ltCountry" runat="server"></asp:Literal></p>
                        </div>
                        <div class="col-md-6">
                            <p><strong>Class:</strong> <asp:Literal ID="ltClass" runat="server"></asp:Literal></p>
                            <p><strong>Price:</strong> $<asp:Literal ID="ltPrice" runat="server"></asp:Literal></p>
                            <p><strong>Release Year:</strong> <asp:Literal ID="ltReleaseYear" runat="server"></asp:Literal></p>
                        </div>
                    </div>
                    
                    <div class="mt-4">
                        <!-- Customer Controls -->
                        <asp:Panel ID="pnlCustomerControls" runat="server" Visible="false">
                            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="btn btn-primary" OnClick="btnAddToCart_Click" />
                        </asp:Panel>
                        
                        <!-- Admin Controls -->
                        <asp:Panel ID="pnlAdminControls" runat="server" Visible="false">
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-warning" OnClick="btnEdit_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClientClick="return confirm('Are you sure you want to delete this jewel?');" OnClick="btnDelete_Click" />
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>