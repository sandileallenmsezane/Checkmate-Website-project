<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Checkmate.com2.Account.Login" Async="true"  %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <section class="login-page">
        <div class="login-container">
            <div class="login-box">
                <h2>LOGIN</h2>

                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>

                <div class="form-container">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Email">USERNAME</asp:Label>
                        <div class="input-field">
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="The email field is required." />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password">PASSWORD</asp:Label>
                        <div class="input-field">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="The password field is required." />
                        </div>
                    </div>

                    <div class="form-options">
                        <div class="remember-me">
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me</asp:Label>
                        </div>
                        <div class="forgot-password">
                            <a href="Forgot.aspx">Forgot Password?</a>
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Button ID="LoginB" runat="server" OnClick="LogIn" Text="Login" CssClass="login-button" />
                    </div>

                    <div class="register-link">
                        <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register as a new user</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <style>
        .login-page {
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: #FFA500;
            padding: 1rem;
        }

        .login-container {
            width: 100%;
            max-width: 400px;
        }

        .login-box {
            background: white;
            padding: 2rem;
            border-radius: 8px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

            .login-box h2 {
                text-align: center;
                margin-bottom: 2rem;
                font-size: 1.5rem;
                font-weight: 600;
                color: #1f2937;
            }

        .form-group {
            margin-bottom: 1.5rem;
        }

            .form-group label {
                display: block;
                font-size: 0.75rem;
                font-weight: 500;
                text-transform: uppercase;
                color: #4b5563;
                margin-bottom: 0.5rem;
            }

        .input-field input {
            width: 100%;
            padding: 0.5rem 0.75rem;
            border: 1px solid #d1d5db;
            border-radius: 4px;
            font-size: 0.875rem;
            transition: border-color 0.2s;
        }

            .input-field input:focus {
                outline: none;
                border-color: #6b7280;
                box-shadow: 0 0 0 2px rgba(107, 114, 128, 0.1);
            }

        .form-options {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 1.5rem;
            font-size: 0.875rem;
        }

        .remember-me {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            color: #4b5563;
        }

        .forgot-password a {
            color: #4b5563;
            text-decoration: none;
        }

            .forgot-password a:hover {
                color: #1f2937;
            }

        .login-button {
            width: 100%;
            padding: 0.75rem;
            background-color: #ff8c00;
            color: white;
            border: none;
            border-radius: 4px;
            font-size: 0.875rem;
            font-weight: 500;
            cursor: pointer;
            transition: background-color 0.2s;
        }

            .login-button:hover {
                background-color: #374151;
            }

        .text-danger {
            color: #dc2626;
            font-size: 0.875rem;
            margin-top: 0.25rem;
        }

        .register-link {
            text-align: center;
            margin-top: 1rem;
        }

            .register-link a {
                color: #4b5563;
                text-decoration: none;
                font-size: 0.875rem;
            }

                .register-link a:hover {
                    color: #1f2937;
                    text-decoration: underline;
                }

        /* Responsive adjustments */
        @media (max-width: 640px) {
            .login-box {
                padding: 1.5rem;
            }
        }
    </style>
</asp:Content>
