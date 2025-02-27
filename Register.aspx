<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Checkmate.com2.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <section class="signup-page">
        <div class="signup-container">
            <div class="signup-box">
                <h2>Welcome To Checkmate</h2>

                <div class="form-container">
                    <asp:ValidationSummary runat="server" CssClass="text-danger" />
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="ErrorMessage" />
                    </p>
                    <div class="form-group">
                        <div class="input-field">
                            <asp:TextBox ID="Username" runat="server" CssClass="form-control"  placeholder="Username"/>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Username" 
                               CssClass="text-danger"  ErrorMessage="Username field is required."/>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="input-field">
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" placeholder="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="The email field is required." />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="input-field">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="The password field is required." />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="input-field">
                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" placeholder="Confirm Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                            <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
                        </div>
                    </div>

                    <div class="terms-group">
                        <label class="terms-label">
                            <input type="checkbox" required />
                            I Agree To The Terms & Conditions
                   
                        </label>
                    </div>

                    <div class="form-group">
                        <asp:Button ID="Registerb" runat="server" OnClick="CreateUser_Click" Text="SIGNUP" CssClass="signup-button" />
                    </div>

                    <div class="login-link">
                        <p>Don't have an Account? Login Now!</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <style>
        .signup-page {
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: #FFA500;
            padding: 1rem;
            position: relative;
            overflow: hidden;
        }

            .signup-page::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: radial-gradient(circle at center, transparent 0%, rgba(255,255,255,0.1) 100%);
                pointer-events: none;
            }

        .signup-container {
            width: 100%;
            max-width: 400px;
            position: relative;
            z-index: 1;
        }

        .signup-box {
            background: rgba(255, 255, 255, 0.1);
            padding: 2rem;
            border-radius: 8px;
            backdrop-filter: blur(10px);
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

            .signup-box h2 {
                text-align: center;
                margin-bottom: 2rem;
                font-size: 1.75rem;
                font-weight: 500;
                color: white;
            }

        .form-group {
            margin-bottom: 1.5rem;
        }

        .input-field input {
            width: 100%;
            padding: 0.75rem;
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 4px;
            background: rgba(255, 255, 255, 0.1);
            color: white;
            font-size: 0.9rem;
            transition: all 0.3s ease;
        }

            .input-field input::placeholder {
                color: rgba(255, 255, 255, 0.7);
            }

            .input-field input:focus {
                outline: none;
                border-color: rgba(255, 255, 255, 0.5);
                background: rgba(255, 255, 255, 0.2);
            }

        .terms-group {
            margin-bottom: 1.5rem;
        }

        .terms-label {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            color: white;
            font-size: 0.875rem;
        }

        .signup-button {
            width: 100%;
            padding: 0.75rem;
            background-color: #ff8c00;
            color: white;
            border: none;
            border-radius: 4px;
            font-size: 0.9rem;
            font-weight: 500;
            cursor: pointer;
            transition: background-color 0.3s;
            text-transform: uppercase;
        }

            .signup-button:hover {
                background-color: #ff7600;
            }

        .login-link {
            text-align: center;
            margin-top: 1.5rem;
        }

            .login-link p {
                color: white;
                font-size: 0.875rem;
                margin: 0;
            }

        .text-danger {
            color: #ff4444;
            font-size: 0.875rem;
            margin-top: 0.25rem;
        }

        /* Responsive adjustments */
        @media (max-width: 640px) {
            .signup-box {
                padding: 1.5rem;
            }
        }
    </style>

</asp:Content>
