<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="Checkmate.com2.Profile.MyProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link href="myprofilestyle.css" rel="stylesheet" />
    <asp:SqlDataSource
        ID="PersonalInfoDataSource"
        runat="server"
        ConnectionString="<%$ ConnectionStrings:GroupWst27Menu %>"
        SelectCommand="SELECT FirstName, LastName, Contact_Number, Birth_Date FROM Customer WHERE (User_ID = @UserID)"
        UpdateCommand="UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, Contact_Number = @Phone, Birth_Date = @BirthDate WHERE (User_ID = @UserID)">
        <SelectParameters>
            <asp:SessionParameter Name="UserID" SessionField="UserID" Type="String" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="FirstName" Type="String" />
            <asp:Parameter Name="LastName" Type="String" />
            <asp:Parameter Name="Phone" Type="String" />
            <asp:Parameter Name="BirthDate" Type="DateTime" />
            <asp:SessionParameter Name="UserID" SessionField="UserID" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource
        ID="EmailDataSource"
        runat="server"
        ConnectionString="<%$ ConnectionStrings:GroupWst27Menu %>"
        UpdateCommand="UPDATE Customer SET Email = @Email WHERE User_ID = @UserID">
        <UpdateParameters>
            <asp:Parameter Name="Email" Type="String" />
            <asp:SessionParameter Name="UserID" SessionField="UserID" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>


    <asp:SqlDataSource
        ID="PasswordDataSource"
        runat="server"
        ConnectionString="<%$ ConnectionStrings:GroupWst27Menu %>"
        UpdateCommand="UPDATE Customer SET Email = @Email WHERE User_ID = @UserID">
        <UpdateParameters>
            <asp:Parameter Name="Email" Type="String" />
            <asp:SessionParameter Name="UserID" SessionField="UserID" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>


    <div class="profile-container">
        <h1>My Account</h1>
        <div class="profile-sidebar">
            <ul class="nav nav-pills nav-stacked">
                <li class="active"><a href="EditProfile.aspx"><i class="fas fa-user"></i> My details</a></li>
                <li><a href="Addresses.aspx"><i class="fas fa-address-book"></i> My address book</a></li>
                <li><a href="OrderHistory.aspx"><i class="fas fa-shopping-bag"></i> My orders</a></li>
            </ul>
        </div>

        <div class="profile-content">
            <h2>My details</h2>
            <div class="section">
                <h3>Personal Information</h3>
                <asp:UpdatePanel ID="PersonalInfoPanel" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="successMessage1" runat="server" ForeColor="Green" Visible="False"></asp:Label>
                        <asp:Label ID="errormessage1" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="FirstName">FIRST NAME</asp:Label>
                                    <asp:TextBox runat="server" ID="FirstName" CssClass="form-control"  Text='<%# Bind("FirstName") %>' />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName"
                                        CssClass="text-danger" ErrorMessage="First name is required." />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="LastName">SECOND NAME</asp:Label>
                                    <asp:TextBox runat="server" ID="LastName" CssClass="form-control" Text='<%# Bind("LastName") %>'/>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="LastName"
                                        CssClass="text-danger" ErrorMessage="Last name is required." />
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="BirthDate">BIRTH DATE</asp:Label>
                            <asp:TextBox runat="server" ID="BirthDate" CssClass="form-control" Text='<%# Bind("BirthDate", "{0:yyyy-MM-dd}") %>'  />
                        </div>

                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="Phone">PHONE NUMBER</asp:Label>
                            <asp:TextBox runat="server" ID="Phone" CssClass="form-control" Text='<%# Bind("Phone") %>'/>
                            <small class="text-muted">Keep 9-digit format with no spaces and dashes.</small>
                        </div>

                        <asp:Button runat="server" ID="UpdatePersonalInfo" Text="SAVE" 
                            CssClass="btn btn-primary" OnClick="UpdatePersonalInfo_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="section">
                <h3>E-mail address</h3>
                <asp:Label ID="successM" runat="server" ForeColor="Green" Visible="False" ></asp:Label>
                <asp:Label ID="errorM" runat="server" ForeColor="Red" Visible="False" ></asp:Label>
                <asp:UpdatePanel ID="EmailPanel" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="Email">E-MAIL ADDRESS</asp:Label>
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" Text='<%# Bind("Email") %>' />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="Email is required." />
                        </div>
                        <asp:Button runat="server" ID="UpdateEmail" Text="SAVE" 
                            CssClass="btn btn-primary" OnClick="UpdateEmail_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="section">
                
                <h3>Password</h3>
                <asp:Label ID="successP" runat="server" ForeColor="Green" Visible="False" ></asp:Label>
                <asp:Label ID="errorM1" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                <asp:Label ID="errorM2" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                <asp:UpdatePanel ID="PasswordPanel" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="CurrentPassword">CURRENT PASSWORD</asp:Label>
                            <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" />
                            <asp:Label ID="errorMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                        </div>
                        

                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="NewPassword">NEW PASSWORD</asp:Label>
                            <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" />
                            <small class="text-muted">Make sure your password is 8 characters long and contains letters and numbers.</small>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="ConfirmPassword">CONFIRM PASSWORD</asp:Label>
                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                            <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" ErrorMessage="The new password and confirmation password do not match." />
                        </div>
                        <asp:Button runat="server" ID="UpdatePassword" Text="SAVE" 
                            CssClass="btn btn-primary" OnClick="UpdatePassword_Click" />
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <style>
        .profile-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }

        .profile-sidebar {
            width: 250px;
            float: left;
            padding-right: 30px;
        }

        .profile-content {
            margin-left: 280px;
        }

        .section {
            margin-bottom: 40px;
            padding-bottom: 20px;
            border-bottom: 1px solid #eee;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .btn-primary {
            background-color: #007bff;
            border: none;
            padding: 10px 30px;
            font-weight: bold;
        }

        .nav-pills > li > a {
            color: #333;
            padding: 10px 15px;
        }

        .nav-pills > li.active > a {
            background-color: #007bff;
        }

        .text-muted {
            font-size: 12px;
            margin-top: 5px;
        }

        /* Responsive design */
        @media (max-width: 768px) {
            .profile-sidebar {
                width: 100%;
                float: none;
                margin-bottom: 30px;
            }

            .profile-content {
                margin-left: 0;
            }
        }
    </style>
</asp:Content>
