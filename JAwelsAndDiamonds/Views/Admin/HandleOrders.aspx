<%@ Page Title="Handle Orders" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="HandleOrders.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Admin.HandleOrders" %>

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
            <h1 class="mb-4">Handle Orders</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3>Unfinished Orders</h3>
                </div>
                <div class="card-body">
                    <asp:Panel ID="pnlNoOrders" runat="server" Visible="false">
                        <p>There are no unfinished orders at the moment.</p>
                    </asp:Panel>
                    
                    <asp:Panel ID="pnlOrders" runat="server">
                        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvOrders_RowCommand" DataKeyNames="TransactionId">
                            <Columns>
                                <asp:BoundField DataField="TransactionId" HeaderText="Order ID" />
                                <asp:BoundField DataField="UserId" HeaderText="User ID" />
                                <asp:BoundField DataField="Username" HeaderText="Username" />
                                <asp:BoundField DataField="TransactionDate" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlPaymentPending" runat="server" Visible='<%# Eval("Status").ToString() == "Payment Pending" %>'>
                                            <asp:Button ID="btnConfirmPayment" runat="server" Text="Confirm Payment" CssClass="btn btn-success btn-sm" CommandName="ConfirmPayment" CommandArgument='<%# Eval("TransactionId") %>' />
                                        </asp:Panel>
                                        
                                        <asp:Panel ID="pnlShipmentPending" runat="server" Visible='<%# Eval("Status").ToString() == "Shipment Pending" %>'>
                                            <asp:Button ID="btnShipPackage" runat="server" Text="Ship Package" CssClass="btn btn-primary btn-sm" CommandName="ShipPackage" CommandArgument='<%# Eval("TransactionId") %>' />
                                        </asp:Panel>
                                        
                                        <asp:Panel ID="pnlArrived" runat="server" Visible='<%# Eval("Status").ToString() == "Arrived" %>'>
                                            <span class="text-info">Waiting for user confirmation...</span>
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