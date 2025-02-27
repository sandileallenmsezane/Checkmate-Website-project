<%@ Page Title="Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="Checkmate.com2.Profile.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="cart-container">
    <h2 class="cart-title">Shopping Cart</h2>
    <asp:Label ID="LabelCartCount" runat="server" Text="" CssClass="cart-count"/>
    
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
        OnRowCommand="GridView1_RowCommand" CssClass="cart-grid"
        GridLines="None" BorderWidth="0px">
        <Columns>
            <asp:BoundField DataField="Prod_Id" HeaderText="ID" />
            <asp:BoundField DataField="Prod_Name" HeaderText="Product" />
            <asp:BoundField DataField="Quantity" HeaderText="Qty" />

            <asp:TemplateField HeaderText="-">
                <ItemTemplate>
                    <asp:Button ID="DecrementButton" runat="server" Text="-" CssClass="qty-btn"
                        CommandName="Decrement" CommandArgument='<%# Eval("Prod_Id") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="+">
                <ItemTemplate>
                    <asp:Button ID="IncrementButton" runat="server" Text="+" CssClass="qty-btn"
                        CommandName="Increment" CommandArgument='<%# Eval("Prod_Id") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="R{0:N2}" />
            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="R{0:N2}" />

            <asp:TemplateField HeaderText="Remove">
                <ItemTemplate>
                    <asp:Button ID="RemoveButton" runat="server" Text="Remove"
                        CommandName="Remove" CommandArgument='<%# Eval("Prod_Id") %>'
                        CssClass="remove-btn" OnClientClick="return confirmRemoval();" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <div class="empty-cart">
                <p>Your cart is empty</p>
                <a href="Shop.aspx" class="continue-shopping">Continue Shopping</a>
            </div>
        </EmptyDataTemplate>
    </asp:GridView>
    
    <div class="cart-footer">
        <asp:Button ID="ContinueShoppingBtn" runat="server" 
            Text="Continue Shopping" 
            CssClass="continue-btn"
           PostBackUrl="~/Shop.aspx" 
             />
            
        <asp:Button ID="CheckoutBtn" runat="server" 
            Text="Proceed to Checkout" 
            CssClass="checkout-btn"
           OnClientClick="window.location.href='Checkout.aspx'; return false;"/>
    </div>
</div>

<style>
    .cart-container {
        max-width: 1200px;
        margin: 20px auto;
        padding: 20px;
        background: white;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .cart-title {
        font-size: 24px;
        color: #333;
        margin-bottom: 20px;
        padding-bottom: 10px;
        border-bottom: 2px solid #eee;
    }

    .cart-count {
        display: block;
        margin-bottom: 20px;
        color: #666;
        font-size: 16px;
    }

    .cart-grid {
        width: 100%;
        border-collapse: collapse;
        margin-bottom: 20px;
    }

    .cart-header th {
        background-color: #f8f9fa;
        padding: 12px;
        text-align: left;
        font-weight: 600;
        color: #333;
        border-bottom: 2px solid #dee2e6;
    }

    .cart-row td {
        padding: 16px 12px;
        vertical-align: middle;
        border-bottom: 1px solid #eee;
    }

    .cart-row:hover {
        background-color: #f8f9fa;
    }

    .id-column {
        width: 60px;
    }

    .product-column {
        min-width: 200px;
    }

    .qty-column {
        width: 120px;
        text-align: center;
    }

    .quantity-container {
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .qty-btn {
        background-color: #007bff;
        color: white;
        border: none;
        padding: 6px 10px;
        cursor: pointer;
        font-size: 14px;
        border-radius: 4px;
        margin: 0 5px;
    }

    .qty-btn:hover {
        background-color: #0056b3;
    }

    .qty-label {
        min-width: 30px;
        text-align: center;
        font-weight: bold;
    }

    .price-column, .subtotal-column {
        width: 120px;
        text-align: right;
    }

    .action-column {
        width: 100px;
        text-align: center;
    }

    .remove-btn {
        padding: 6px 12px;
        background-color: #dc3545;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        transition: background-color 0.2s;
    }

    .remove-btn:hover {
        background-color: #c82333;
    }

    .empty-cart {
        text-align: center;
        padding: 40px;
        color: #666;
    }

    .cart-footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-top: 20px;
        padding-top: 20px;
        border-top: 2px solid #eee;
    }

    .continue-btn, .checkout-btn {
        padding: 12px 24px;
        border-radius: 4px;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
    }

    .continue-btn {
        background-color: #f8f9fa;
        border: 1px solid #dee2e6;
        color: #333;
    }

    .continue-btn:hover {
        background-color: #e2e6ea;
    }

    .checkout-btn {
        background-color: black;
        border: none;
        color: white;
    }

    .checkout-btn:hover {
        background-color: #218838;
    }

    .continue-shopping {
        display: inline-block;
        margin-top: 10px;
        padding: 8px 16px;
        background-color: #007bff;
        color: white;
        text-decoration: none;
        border-radius: 4px;
    }

    .continue-shopping:hover {
        background-color: #0056b3;
        text-decoration: none;
    }
</style>

<script type="text/javascript">
    function confirmRemoval() {
        return confirm('Are you sure you want to remove this item from your cart?');
    }
</script>
    
</asp:Content>

