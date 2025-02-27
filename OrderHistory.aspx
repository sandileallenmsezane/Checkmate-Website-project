<%@ Page Title="My orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderHistory.aspx.cs" Inherits="Checkmate.com2.Profile.OrderHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="StatusMessage" runat="server" CssClass="status-message"></asp:Label>
    <style>
        .container {
            width: 80%;
            margin: auto;
            font-family: Arial, sans-serif;
        }

        .grid-container {
            display: flex;
            justify-content: space-between;
        }

        .grid {
            width: 48%;
            display: none;
        }

        /* Styling for GridView Rows */
        .gridview-container table {
            width: 100%;
            border-collapse: collapse;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
        }

        .gridview-container th, .gridview-container td {
            padding: 10px;
            text-align: left;
            border-bottom: 1px solid #ddd;
        }

        .gridview-container tr:hover {
            background-color: #f5f5f5;
            transition: background-color 0.3s ease-in-out;
        }
    </style>
    <!-- Section for the order summary -->

    <div class="container">
        <h2>Total Orders</h2>
        <div class="gridview-container">
            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="gridview" OnSelectedIndexChanged="gvOrders_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="Order Number" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Date of Purchase" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:CommandField ShowSelectButton="True" SelectText="View Details" />
                </Columns>
            </asp:GridView>
        </div>
        <h2>Order Details</h2>
        <div id="orderDetailsContainer" class="gridview-container" style="display: none;">
            <asp:GridView ID="gvOrderDetails" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="ID" />
                    <asp:BoundField DataField="Prod_Description" HeaderText="Name" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <script>
        function showOrderDetails() {
            document.getElementById("orderDetailsContainer").style.display = "block";
        }
    </script>
</asp:Content>

