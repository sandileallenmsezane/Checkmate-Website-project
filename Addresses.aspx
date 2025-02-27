<%@ Page Title="Address Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Addresses.aspx.cs" Inherits="Checkmate.com2.Profile.Addresses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
    <div class="container">
        <h2>Manage Address</h2>
        <div class="grid-container">
            <asp:GridView ID="gvAddress" runat="server" AutoGenerateColumns="False" CssClass="gridview" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                <Columns>
                    <asp:BoundField DataField="addresses" HeaderText="Address" />
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:BoundField DataField="ZIP" HeaderText="Postal Code" />
                    <asp:BoundField DataField="Province" HeaderText="Province" />
                </Columns>
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />
            </asp:GridView>
        </div>
    </div>
    
    
</asp:Content>
