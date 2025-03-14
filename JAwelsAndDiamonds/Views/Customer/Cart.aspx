<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Customer.Cart" %>

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
            <h1 class="mb-4">Shopping Cart</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3>Your Cart Items</h3>
                </div>
                <div class="card-body">
                    <asp:Panel ID="pnlEmptyCart" runat="server" Visible="false">
                        <p>Your cart is empty. <a href="~/Views/Home/Index.aspx" runat="server">Continue shopping</a></p>
                    </asp:Panel>

                    <asp:Panel ID="pnlCartItems" runat="server">
                        <asp:GridView ID="gvCartItems" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvCartItems_RowCommand" DataKeyNames="JewelId">
                            <Columns>
                                <asp:BoundField DataField="JewelId" HeaderText="Jewel ID" />
                                <asp:BoundField DataField="JewelName" HeaderText="Jewel Name" />
                                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="BrandName" HeaderText="Brand" />
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="form-control" TextMode="Number" min="1"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-warning btn-sm" CommandName="UpdateItem" CommandArgument='<%# Eval("JewelId") %>' />
                                        <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-danger btn-sm" CommandName="RemoveItem" CommandArgument='<%# Eval("JewelId") %>' OnClientClick="return confirm('Are you sure you want to remove this item?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="d-flex justify-content-between mt-4">
                            <div>
                                <asp:Button ID="btnClearCart" runat="server" Text="Clear Cart" CssClass="btn btn-outline-danger" OnClick="btnClearCart_Click" OnClientClick="return confirm('Are you sure you want to clear your cart?');" />
                            </div>
                            <div class="text-end">
                                <h4>Total:
                                    <asp:Label ID="lblTotal" runat="server" Text="$0.00"></asp:Label></h4>
                            </div>
                        </div>

                        <div class="mt-4">
                            <div class="form-group">
                                <label for="ddlPaymentMethod" class="form-label">Payment Method</label>
                                <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="-- Select Payment Method --" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="mt-3">
                                <asp:Button ID="btnCheckout" runat="server" Text="Checkout" CssClass="btn btn-primary" OnClick="btnCheckout_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
