using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Checkmate.com2.Models;
using System.Net.Mail;
using System.Configuration;

namespace Checkmate.com2.Account
{
    public partial class ForgotPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Forgot(object sender, EventArgs e)
        {
            string email = Email.Text.Trim();
            string resetToken = Guid.NewGuid().ToString(); // Generate a unique reset token

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GroupWst27Menu"].ConnectionString))
            {
                con.Open();
                string query = "SELECT UserID FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    object userId = cmd.ExecuteScalar();

                    if (userId != null) // If email exists in the database
                    {
                        // Store token in the database
                        string updateQuery = "UPDATE Users SET ResetToken = @Token, TokenExpiry = DATEADD(HOUR, 1, GETDATE()) WHERE Email = @Email";
                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@Token", resetToken);
                            updateCmd.Parameters.AddWithValue("@Email", email);
                            updateCmd.ExecuteNonQuery();
                        }

                        // Generate Reset Link
                        string resetLink = Request.Url.GetLeftPart(UriPartial.Authority) +
                                           "/ResetPassword.aspx?email=" + HttpUtility.UrlEncode(email) +
                                           "&token=" + HttpUtility.UrlEncode(resetToken);

                        // Send Email
                        SendResetEmail(email, resetLink);
                    }
                }

                string equery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(equery, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0) // Email exists
                    {
                        Response.Redirect("ResetPassword.aspx?email=" + email);
                    }
                    else
                    {
                        FailureText.Text = "Email not found.";
                        ErrorMessage.Visible = true;
                    }
                }
            }

            // Always show the same message to prevent email enumeration
            loginForm.Visible = false;
            DisplayEmail.Visible = true;
        }


        private void SendResetEmail(string email, string resetLink)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("your_email@example.com"); // Change to your email
                mail.Subject = "Password Reset Request";
                mail.Body = $"Click <a href='{resetLink}'>here</a> to reset your password.";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.example.com"); // Change to your SMTP server
                smtp.Credentials = new System.Net.NetworkCredential("your_email@example.com", "your_password"); // Use valid credentials
                smtp.EnableSsl = true;
                smtp.Send(mail);



            }
            catch (Exception ex)
            {
                FailureText.Text = "Error sending email: " + ex.Message;
                ErrorMessage.Visible = true;
            }
        }
    }
}