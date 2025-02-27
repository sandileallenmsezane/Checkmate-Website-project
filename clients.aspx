<%@ Page Title="Clients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="clients.aspx.cs" Inherits="Checkmate.com2.admin.clients" %>
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
    </style>
    
    <h2>Manage Customers</h2>

    <%-- Search Box --%>
    <div>
        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by Name or Email"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
    </div>

    <%-- Customer Table with Pagination --%>
    <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="False" DataKeyNames="Customer_ID"
        CssClass="table" AllowPaging="True" PageSize="5" OnPageIndexChanging="gvCustomers_PageIndexChanging"
        OnRowEditing="gvCustomers_RowEditing" OnRowUpdating="gvCustomers_RowUpdating"
        OnRowCancelingEdit="gvCustomers_RowCancelingEdit" OnRowDeleting="gvCustomers_RowDeleting">
        <Columns>
            <asp:BoundField DataField="Customer_ID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Contact_Number" HeaderText="Phone" />
            <asp:BoundField DataField="Birth_Date" HeaderText="BirthDate" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
