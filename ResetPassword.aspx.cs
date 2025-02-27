using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Checkmate.com2.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Checkmate.com2.Account
{
    public partial class ResetPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string email = Request.QueryString["email"];
                string token = Request.QueryString["token"];

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                {
                    ErrorMessage.Text = "Correct reset link.";
                    return;
                }

                Email.Text = email; // Pre-fill the email field
                Email.ReadOnly = true; // Make it read-only to prevent changes
            }
        }

        protected string StatusMessage
        {
            get;
            private set;

        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            string email = Request.QueryString["email"];
            string token = Request.QueryString["token"];
            string newPassword = Password.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                ErrorMessage.Text = "Invalid reset request.";
                return;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GroupWst27Menu"].ConnectionString))
            {
                con.Open();
                string query = "SELECT UserID FROM Users WHERE Email = @Email AND ResetToken = @Token AND TokenExpiry > GETDATE()";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Token", token);
                    object userId = cmd.ExecuteScalar();

                    if (userId != null) // Token is valid
                    {
                        // Hash the password using SHA-256
                        string hashedPassword = ComputeSHA256Hash(newPassword);

                        string updateQuery = "UPDATE Users SET Password = @Password, ResetToken = NULL, TokenExpiry = NULL WHERE Email = @Email";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@Password", hashedPassword);
                            updateCmd.Parameters.AddWithValue("@Email", email);
                            updateCmd.ExecuteNonQuery();
                        }

                        // Redirect to login page after successful password reset
                        Response.Redirect("Login.aspx?message=reset_success");
                    }
                    else
                    {
                        ErrorMessage.Text = "Invalid or expired token.";
                    }
                }
            }
        }

        // Method to compute SHA-256 hash
        private string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}