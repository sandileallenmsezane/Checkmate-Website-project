using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNet.Identity; // For PasswordHasher
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Checkmate.com2.Models;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Collections.Generic;

namespace Checkmate.com2.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set the register link URL
                RegisterHyperLink.NavigateUrl = "Register.aspx";
            }
            
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=146.230.177.46;Initial Catalog=GroupWst27;Persist Security Info=True;User ID=GroupWst27;Password=mhfd5"))
                    {
                        conn.Open();

                        string hashedPassword = HashPassword(Password.Text.Trim());

                        string query = @"SELECT UserID, Username, Email, Role 
                               FROM Users 
                               WHERE Email = @Email AND Password = @Password";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Email", Email.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", hashedPassword);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();

                                    string username = reader["Username"].ToString();
                                    string email = reader["Email"].ToString();
                                    string userId = reader["UserID"].ToString();
                                    string role = reader["Role"].ToString();

                                    // Store in session
                                    Session["UserID"] = userId;
                                    Session["Username"] = username;
                                    Session["Email"] = email;
                                    Session["Role"] = role;

                                    // Create Identity Claims
                                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, username),
                                new Claim(ClaimTypes.Email, email),
                                new Claim(ClaimTypes.NameIdentifier, userId),
                                new Claim(ClaimTypes.Role, role)
                            };

                                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                                    var principal = new ClaimsPrincipal(identity);

                                    // Sign in using OWIN authentication
                                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                                    authenticationManager.SignIn(new AuthenticationProperties
                                    {
                                        IsPersistent = RememberMe.Checked
                                    }, identity);

                                    if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                                    {
                                        Response.Redirect(Request.QueryString["ReturnUrl"]);
                                    }
                                    else
                                    {
                                        switch (role.ToLower())
                                        {
                                            case "admin":
                                                Response.Redirect("~/admin/defaultadmin.aspx");
                                                break;
                                            case "user":
                                                Response.Redirect("~/Default.aspx");
                                                break;
                                            default:
                                                Response.Redirect("~/Default.aspx");
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    FailureText.Text = "Invalid email or password.";
                                    ErrorMessage.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                    FailureText.Text = "An error occurred during login. Please try again.";
                    ErrorMessage.Visible = true;
                }
            }
        }

        // Make sure to use the exact same hashing method as in registration
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            Uri absoluteUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                return String.Equals(this.Request.Url.Host, absoluteUri.Host,
                                   StringComparison.OrdinalIgnoreCase);
            }

            bool isLocal = url[0] == '/' && (url.Length == 1 ||
                                            url[1] != '/' && url[1] != '\\') ||
                          url.Length > 1 &&
                          url[0] == '~' && url[1] == '/';

            return isLocal;
        }
    }
}