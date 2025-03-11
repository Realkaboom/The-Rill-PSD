<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="MyOrders.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Customer.MyOrders" %>

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
            <h1 class="mb-4">My Orders</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3>Order History</h3>
                </div>
                <div class="card-body">
                    <asp:Panel ID="pnlNoOrders" runat="server" Visible="false">
                        <p>You have no orders yet. <a href="~/Views/Home/Index.aspx" runat="server">Start shopping</a></p>
                    </asp:Panel>
                    
                    <asp:Panel ID="pnlOrders" runat="server">
                        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvOrders_RowCommand" DataKeyNames="TransactionId">
                            <Columns>
                                <asp:BoundField DataField="TransactionId" HeaderText="Order ID" />
                                <asp:BoundField DataField="TransactionDate" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="PaymentMethodName" HeaderText="Payment Method" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:Button ID="btnViewDetails" runat="server" Text="View Details" CssClass="btn btn-info btn-sm" CommandName="ViewDetails" CommandArgument='<%# Eval("TransactionId") %>' />
                                        
                                        <asp:Panel ID="pnlArrivedActions" runat="server" Visible='<%# Eval("Status").ToString() == "Arrived" %>'>
                                            <asp:Button ID="btnConfirmPackage" runat="server" Text="Confirm Package" CssClass="btn btn-success btn-sm" CommandName="ConfirmPackage" CommandArgument='<%# Eval("TransactionId") %>' />
                                            <asp:Button ID="btnRejectPackage" runat="server" Text="Reject Package" CssClass="btn btn-danger btn-sm" CommandName="RejectPackage" CommandArgument='<%# Eval("TransactionId") %>' OnClientClick="return confirm('Are you sure you want to reject this package?');" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>