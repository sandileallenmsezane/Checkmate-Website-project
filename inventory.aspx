<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="inventory.aspx.cs" Inherits="Checkmate.com2.admin.inventory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

            .table th, .table td {
                border: 1px solid #ddd;
                padding: 8px;
                text-align: left;
            }

            .table th {
                background-color: #333;
                color: white;
            }

        input[type="text"] {
            padding: 5px;
            width: 200px;
        }

        button {
            padding: 8px;
            background-color: #28a745;
            color: white;
            border: none;
            cursor: pointer;
        }

            button:hover {
                background-color: #218838;
            }

        .btn-primary {
            background-color: #007bff;
            border: none;
            padding: 10px 30px;
            font-weight: bold;
        }
    </style>

    <h2>Manage Inventory</h2>

    <div>
        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by Name or Email"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
    </div>
    <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="False" DataKeyNames="Prod_ID" 
        CssClass="table" AllowPaging="True" PageSize="5" OnPageIndexChanging="gvInventory_PageIndexChanging"
        OnRowEditing="gvInventory_RowEditing" OnRowDeleting="gvInventory_RowDeleting"
        OnRowUpdating="gvInventory_RowUpdating" OnRowCancelingEdit="gvInventory_RowCancelingEdit">
        <Columns>
            <asp:BoundField DataField="Prod_ID" HeaderText="Product ID" ReadOnly="True" />
            <asp:TemplateField HeaderText="Product Name">
                <ItemTemplate>
                    <%# Eval("Prod_Description") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtProductName" runat="server" Text='<%# Bind("Prod_Description") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Price">
                <ItemTemplate>
                    <%# Eval("Prod_Price") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("Prod_Price") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <%# Eval("Prod_Quantity_Available") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Bind("Prod_Quantity_Available") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Category">
                <ItemTemplate>
                    <%# Eval("Category") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtCategory" runat="server" Text='<%# Bind("Category") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

    <!-- Insert New Product Form -->
    <%--<asp:Button ID="btnAddProduct" runat="server" Text="Add Product" CssClass="btn btn-primary"  />--%>
    <asp:TextBox ID="txtProductName" runat="server" Placeholder="Product Name" />
    <asp:TextBox ID="txtPrice" runat="server" Placeholder="Price" />
    <asp:TextBox ID="txtQuantity" runat="server" Placeholder="Quantity" />
    <asp:TextBox ID="txtCategory" runat="server" PlaceHolder="Category"></asp:TextBox>
    <asp:Button ID="btnInsertProduct" runat="server" Text="Insert Product" CssClass="btn btn-primary" OnClick="btnInsertProduct_Click" />

</asp:Content>
