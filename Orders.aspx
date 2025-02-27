<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="Checkmate.com2.Profile.Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblMessage" runat="server" CssClass="status-message"></asp:Label>

    <div class="order-details-container">
        <h2 class="page-title">Order Details</h2>

        <!-- Order Information -->
        <div class="order-info">
            <h3>Order Information</h3>
            <p><strong>Customer Name:</strong>
                <asp:Label ID="lblCustomerName" runat="server"></asp:Label></p>
            <p><strong>Email:</strong>
                <asp:Label ID="lblEmail" runat="server"></asp:Label></p>
            <p><strong>Phone:</strong>
                <asp:Label ID="lblPhone" runat="server"></asp:Label></p>
            <p><strong>Address:</strong>
                <asp:Label ID="lblAddress" runat="server"></asp:Label></p>
            <p><strong>City:</strong>
                <asp:Label ID="lblCity" runat="server"></asp:Label></p>
            <p><strong>Postal Code:</strong>
                <asp:Label ID="lblPostalCode" runat="server"></asp:Label></p>
            <p><strong>Order Date:</strong>
                <asp:Label ID="lblOrderDate" runat="server"></asp:Label></p>
            <p><strong>Total Amount:</strong>
                <asp:Label ID="lblTotalAmount" runat="server"></asp:Label></p>
            <p><strong>Delivery Fee:</strong>
                <asp:Label ID="lblDeliveryFee" runat="server"></asp:Label></p>
            <p><strong>Status:</strong>
                <asp:Label ID="lblStatus" runat="server"></asp:Label></p>
        </div>

        <hr />

        <!-- Items Ordered -->
        <div class="order-items">
            <h3>Items Ordered</h3>
            <asp:Label ID="OrderId" runat="server" Text="Label"></asp:Label>
            <asp:GridView ID="gvOrderDetails" runat="server" AutoGenerateColumns="False" CssClass="styled-table">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <style>
        /* Styling for the entire container */
        .order-details-container {
            font-family: Arial, sans-serif;
            max-width: 800px;
            margin: 20px auto;
            padding: 20px;
            background-color: #f9f9f9;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        .page-title {
            text-align: center;
            color: #333;
            margin-bottom: 20px;
        }

        .order-info {
            margin-bottom: 20px;
            padding: 10px 15px;
            border-left: 4px solid #4CAF50;
            background-color: #fff;
        }

            .order-info p {
                margin: 5px 0;
                font-size: 16px;
            }

            .order-info strong {
                color: #4CAF50;
            }

        hr {
            border: none;
            height: 1px;
            background-color: #ddd;
            margin: 20px 0;
        }

        .order-items h3 {
            margin-bottom: 10px;
            color: #333;
        }

        .styled-table {
            width: 100%;
            border-collapse: collapse;
            margin: 15px 0;
            font-size: 16px;
            text-align: left;
            background-color: #fff;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

            .styled-table th,
            .styled-table td {
                padding: 12px 15px;
                border: 1px solid #ddd;
            }

            .styled-table th {
                background-color: #4CAF50;
                color: white;
                text-align: center;
            }

            .styled-table td {
                text-align: center;
            }

            .styled-table tr:nth-child(even) {
                background-color: #f2f2f2;
            }

            .styled-table tr:hover {
                background-color: #ddd;
            }

        .status-message {
            text-align: center;
            font-size: 16px;
            margin-bottom: 20px;
            padding: 10px;
            border-radius: 5px;
        }

        .success-message {
            background-color: #dff0d8;
            color: #3c763d;
        }

        .error-message {
            background-color: #f2dede;
            color: #a94442;
        }
    </style>


</asp:Content>
