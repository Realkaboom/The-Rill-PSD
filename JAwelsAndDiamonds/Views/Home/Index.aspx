<%@ Page Title="Home" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Home.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AlertContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h1 class="mb-4">Welcome to JAwels & Diamonds</h1>
            <p class="lead">Discover our exquisite collection of luxury jewelry.</p>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3>Our Jewels</h3>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="rptJewels" runat="server">
                        <HeaderTemplate>
                            <div class="row">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="col-md-4 mb-4">
                                <div class="card h-100">
                                    <div class="card-body">
                                        <h5 class="card-title"><%# Eval("JewelName") %></h5>
                                        <p class="card-text">
                                            <strong>ID:</strong> <%# Eval("JewelId") %><br />
                                            <strong>Price:</strong> $<%# Eval("Price", "{0:0.00}") %>
                                        </p>
                                        <a href="JewelDetail.aspx?id=<%# Eval("JewelId") %>" class="btn btn-primary">View Details</a>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>