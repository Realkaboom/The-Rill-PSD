<%@ Page Title="Transaction Reports" Language="C#" MasterPageFile="~/Views/Shared/_Layout.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="JAwelsAndDiamonds.Views.Admin.Reports" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AlertContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h1 class="mb-4">Transaction Reports</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header">
                    <h3>Successful Transactions (Status: Done)</h3>
                </div>
                <div class="card-body">
                    <div class="report-container">
                        <CR:CrystalReportViewer ID="crvTransactions" runat="server" AutoDataBind="true" 
                            ToolPanelView="None" Width="100%" Height="600px" HasToggleGroupTreeButton="False"
                            HasToggleParameterPanelButton="False" ToolbarStyle-BackColor="White" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>