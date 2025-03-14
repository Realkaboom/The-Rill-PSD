<%@ Page Title="Transaction Detail" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="TransactionDetail.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Customer.TransactionDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AlertContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h1 class="mb-4">Transaction Detail</h1>
            <p><a href="MyOrders.aspx" class="btn btn-outline-secondary">Back to My Orders</a></p>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3>Order #<asp:Literal ID="ltTransactionId" runat="server"></asp:Literal></h3>
                </div>
                <div class="card-body">
                    <div class="row mb-4">
                        <div class="col-md-6">
                            <p><strong>Date:</strong> <asp:Literal ID="ltTransactionDate" runat="server"></asp:Literal></p>
                            <p><strong>Payment Method:</strong> <asp:Literal ID="ltPaymentMethod" runat="server"></asp:Literal></p>
                        </div>
                        <div class="col-md-6">
                            <p><strong>Status:</strong> <asp:Literal ID="ltStatus" runat="server"></asp:Literal></p>
                        </div>
                    </div>
                    
                    <h4>Items</h4>
                    <div class="table-responsive">
                        <asp:GridView ID="gvTransactionDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="JewelId" HeaderText="Jewel ID" />
                                <asp:BoundField DataField="JewelName" HeaderText="Jewel Name" />
                                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    
                    <div class="text-end mt-3">
                        <h4>Total: <asp:Literal ID="ltTotal" runat="server"></asp:Literal></h4>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>