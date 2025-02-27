<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="defaultadmin.aspx.cs" Inherits="Checkmate.com2.admin.defaultadmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .navbar {
            background: #333;
            padding: 10px;
        }

            .navbar a {
                color: white;
                margin-right: 15px;
                text-decoration: none;
            }

        .dashboard-grid {
            display: flex;
            gap: 20px;
        }

        .card {
            background: #f4f4f4;
            padding: 20px;
            border-radius: 8px;
        }
    </style>
    
    
    <h2>Welcome to Admin Dashboard</h2>
    
    <div class="dashboard-grid">
        <div class="card">
            <h3>Total Customers</h3>
            <asp:Label ID="lblTotalCustomers" runat="server" Text="0"></asp:Label>
        </div>
        <div class="card">
            <h3>Total Products</h3>
            <asp:Label ID="lblTotalProducts" runat="server" Text="0"></asp:Label>
        </div>
        <div class="card">
            <h3>Total Sales</h3>
            <asp:Label ID="lblTotalSales" runat="server" CssClass="value-label" Text="0"></asp:Label>
        </div>
    </div>


</asp:Content>
