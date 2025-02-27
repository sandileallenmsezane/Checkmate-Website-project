<%@ Page Title="Shopping" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="Checkmate.com2.Shop" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            display: flex;
            flex-direction: column;
            min-height: 100vh;
            margin: 0;
        }

        .shop-container {
            display: flex;
            flex-direction: row;
            gap: 20px;
            padding: 20px;
            min-height: 100vh;
        }

        .category-panel {
            flex: 0 0 250px;
            background: #f8f9fa;
            padding: 20px;
            border-right: 1px solid #ddd;
            height: 100vh;
            position: sticky;
            top: 0;
        }

        .category-button{
            padding: 10px 20px;
            background: #FFA500;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.2s;
        }
            .category-button:hover{
                background: #0056b3;
            }

        .category-list {
            list-style-type: none;
            padding: 0;
            margin: 0;
        }

        .category-item {
            display: flex;
            align-items: center;
            margin-bottom: 8px;
        }

        .category-radio {
            margin-right: 10px;
        }


        .category-items a {
            display: block;
            padding: 10px;
            font-size: 16px;
            color: #333;
            text-decoration: none;
            cursor: pointer;
            margin: 2px 0;
        }

            .category-items a:hover {
                background-color: #007bff;
                color: white;
                border-radius: 4px;
            }


        .search-section {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
            padding: 15px;
            background: #f8f9fa;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
        }

        .search-input {
            flex: 1;
            padding: 10px 15px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 16px;
            outline: none;
            transition: border-color 0.2s;
        }

            .search-input:focus {
                border-color: #007bff;
                box-shadow: 0 0 0 2px rgba(0,123,255,0.1);
            }

        .search-button {
            padding: 10px 20px;
            background: #FFA500;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.2s;
        }

            .search-button:hover {
                background: #0056b3;
            }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .search-section {
                flex-direction: column;
            }

            .search-button {
                width: 100%;
            }


        }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .search-section {
                flex-direction: column;
            }

            .search-button {
                width: 100%;
            }
        }

        .product-content {
            flex: 1;
            padding: 20px;
        }

        .menu-grid {
            width: 100%;
        }

        /* Style for DataList */
        #DataList1 {
            width: 100%;
            display: table;
        }

        .product-card {
            padding: 15px;
            margin: 10px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background: white;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        .product-image {
            width: 100%;
            height: 200px;
            object-fit: cover;
            border-radius: 4px;
        }

        .product-name {
            margin: 10px 0;
            font-size: 1.1em;
            color: #333;
        }

        .product-price {
            display: block;
            color: #28a745;
            font-weight: bold;
            margin: 10px 0;
        }

        .add-to-cart-btn {
            width: 100%;
            max-width: 150px;
            display: block;
            margin: 10px auto;
        }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .shop-container {
                flex-direction: column;
            }

            .category-panel {
                width: 100%;
                height: auto;
                position: static;
            }

            #DataList1 {
                width: 100%;
            }
        }

        /* Pager Container */
        .pager-container {
            text-align: center;
            margin-top: 20px;
        }

        /* Pager Button Styling */
        .pager-btn {
            padding: 10px 20px;
            margin: 0 5px;
            border: none;
            background-color: #FFA500;
            color: white;
            font-size: 14px;
            cursor: pointer;
            border-radius: 5px;
            transition: background-color 0.3s ease;
        }

            .pager-btn:disabled {
                background-color: #ccc;
                cursor: not-allowed;
            }

            .pager-btn:hover:not(:disabled) {
                background-color: #0056b3;
            }

        /* Footer Pager Styling */
        .pager-footer {
            background-color: #f8f9fa;
            text-align: center;
            padding: 15px 0;
            border-top: 1px solid #ddd;
            width: 100%;
            position: relative;
            box-shadow: 0 -2px 5px rgba(0, 0, 0, 0.1);
            bottom: 0;
        }
    </style>
    <asp:Label ID="StatusMessage" runat="server" CssClass="status-message"></asp:Label>
    <div class="shop-container">

        <aside class="category-panel">
            <asp:Panel ID="CategoryPanel" runat="server" CssClass="category-list">
                <h1>Category</h1>
                <asp:Repeater ID="CategoryRepeater" runat="server" OnItemCommand="CategoryRepeater_ItemCommand">
                    <ItemTemplate>
                        <div class="category-item">
                            <asp:RadioButton ID="CategoryRadio" runat="server"
                                GroupName="CategoryGroup"
                                AutoPostBack="true"
                                Text='<%# Eval("Category") %>'
                                OnCheckedChanged="CategoryRadio_CheckedChanged"
                                CssClass="category-radio"
                                CommandArgument='<%# Eval("Category") %>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Button ID="AllCategoriesButton" runat="server" Text="All Categories"
                    CommandName="FilterCategory" CommandArgument="All"
                    OnClick="AllCategoriesButton_Click" CssClass="category-button"/>
                <asp:SqlDataSource ID="CategoryDataSource" runat="server"
                    ConnectionString="<%$ ConnectionStrings:GroupWst27Menu %>"
                    SelectCommand="SELECT DISTINCT Category AS CategoryName, Category FROM Product_Item"></asp:SqlDataSource>
            </asp:Panel>
        </aside>

        <main class="product-content">
            <div class="search-section">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" placeholder="Search products..."></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="search-button" OnClick="btnSearch_Click" />
            </div>
             <asp:Label ID="LabelCartCount" runat="server" CssClass="status-message"></asp:Label>
            <section class="menu-grid">
                <asp:SqlDataSource
                    ID="SqlDataSource1"
                    runat="server"
                    ConnectionString="<%$ ConnectionStrings:GroupWst27Menu %>"
                    SelectCommand="SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Prod_ID) AS RowNum, [Prod_ID], [ImageUrl], [Prod_Description], [Prod_Price] FROM [Product_Item]) AS T WHERE RowNum BETWEEN @StartRow AND @EndRow">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DataList1" DefaultValue="1" Name="StartRow" PropertyName="SelectedValue" Type="Int32" />
                        <asp:ControlParameter ControlID="DataList1" DefaultValue="9" Name="EndRow" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:DataList ID="DataList1" runat="server" RepeatColumns="3"
                    DataKeyField="Prod_ID"
                    OnItemCommand="DataList1_ItemCommand">

                    <ItemTemplate>
                        <!-- Product Card -->
                        <div class="product-card">
                            <asp:Image ID="ProductImage" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' CssClass="product-image" />
                            <h4 class="product-name"><%# Eval("Prod_Description") %></h4>
                            <asp:Label ID="ProductPriceLabel" runat="server" Text='<%# Eval("Prod_Price") %>' CssClass="product-price"></asp:Label>
                            <asp:Label ID="LabelThresold" runat="server" Text=" "></asp:Label>
                            <asp:ImageButton ID="ImageButton1" runat="server"
                                ImageUrl="~/MenuPics/sisu button.png"
                                CssClass="add-to-cart-btn" CommandArgument='<%# Eval("Prod_ID") %>' OnClick="ImageButton1_Click" />
                        </div>
                    </ItemTemplate>

                    <SelectedItemStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                </asp:DataList>
            </section>
        </main>
    </div>
    <footer class="pager-footer">
        <div class="pager-container">
            <asp:Button ID="PreviousButton" runat="server" Text="Previous" OnClick="PreviousPage_Click" CssClass="pager-btn" />
            <asp:Button ID="NextButton" runat="server" Text="Next" OnClick="NextPage_Click" CssClass="pager-btn" />
        </div>
    </footer>
</asp:Content>
