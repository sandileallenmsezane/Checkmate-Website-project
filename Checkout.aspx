<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="Checkmate.com2.Profile.Shopping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="StatusMessage" runat="server" CssClass="status-message"></asp:Label>
    <style>
        body {
            background-color: #FFF5E1;
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
        }
        .checkout-container {
            display: flex;
            justify-content: space-between;
            max-width: 900px;
            margin: auto;
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }

        .checkout-form, .order-summary, .payment-container{
            flex: 1;
            margin-right: 20px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-label {
            font-weight: bold;
            display: block;
            margin-bottom: 5px;
        }
        .form-control {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }
        .order-summary {
            background: #f9f9f9;
            padding: 15px;
            border-radius: 8px;
        }
        .summary-item {
            display: flex;
            justify-content: space-between;
            margin-bottom: 10px;
        }
        .summary-total {
            font-weight: bold;
        }
        .payment-options {
            display: flex;
            flex-direction: column;
            margin-bottom: 15px;
        }
        .payment-options img {
            height: 20px;
            margin-left: 5px;
        }
        .pay-button {
            width: 100%;
            background-color: #28a745;
            color: white;
            border: none;
            padding: 10px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }
    </style>

    <div class="checkout-container">
        <div class="checkout-form">
            <h2>Billing Address</h2>
            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="First Name"></asp:Label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="Last Name"></asp:Label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="Email"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="Address"></asp:Label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="City"></asp:Label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>

            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="Postal Code"></asp:Label>
                <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="Provinces"></asp:Label>
                <asp:TextBox ID="txtProvince" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="order-summary">
            <h2>Your Cart</h2>
            <asp:Repeater ID="OrderItemsRepeater" runat="server">
                <ItemTemplate>
                    <div class="summary-item">
                        <span><%# Eval("Prod_Name") %> (<%# Eval("Quantity") %>)</span>
                        <span>R <%# Eval("SubTotal", "{0:N2}") %></span>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="summary-item summary-total">
                <span>Total (RAND)</span>
                <span>R <asp:Literal ID="litTotal" runat="server"></asp:Literal></span>
            </div>
        </div>
    </div>
    <div class="payment-container">
        <h2>Payment Method</h2>
        <div class="payment-options">
            <asp:Label runat="server" CssClass="form-label">
                <asp:RadioButton ID="rbPaypal" runat="server" GroupName="paymentMethod" AutoPostBack="true" OnCheckedChanged="PaymentMethodChanged" />
                Paypal <img src="paypal-logo.png" alt="Paypal">
            </asp:Label>
            <asp:Label runat="server" CssClass="form-label">
                <asp:RadioButton ID="rbCard" runat="server" GroupName="paymentMethod" AutoPostBack="true" OnCheckedChanged="PaymentMethodChanged" />
                Debit/Credit Card <img src="visa.png" alt="Visa">
            </asp:Label>
        </div>
        <div class="card-details">
            <asp:Panel ID="pnlCardDetails" runat="server" Visible="false">
                <asp:Label runat="server" Text="Name on Card" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Placeholder="e.g. Richard Bovell"></asp:TextBox>
                <asp:Label runat="server" Text="Card Number" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" Placeholder="8888-8888-8888-8888"></asp:TextBox>
                <asp:Label runat="server" Text="Expiry Date" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtExpiry" runat="server" CssClass="form-control" Placeholder="MM/YY"></asp:TextBox>
                <asp:Label runat="server" Text="CVC/CVV" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtCVC" runat="server" CssClass="form-control" Placeholder="123"></asp:TextBox>
            </asp:Panel>

        </div>
        <asp:Button ID="btnPay" runat="server" CssClass="pay-button" Text="Confirm and Pay"
            OnClick="btnPay_Click" OnClientClick="window.location.href='Cart.aspx'; return false;"/>
    </div>
</asp:Content>
