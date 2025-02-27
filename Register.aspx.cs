using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Checkmate.com2.Models;
using System.Security.Cryptography;
using System.Text;

namespace Checkmate.com2.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Username.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                string code = manager.GenerateEmailConfirmationToken(user.Id);
                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                // Hash the password
                string hashedPassword = HashPassword(Password.Text);

                // Insert details into Users table
                DataSet1TableAdapters.Users1TableAdapter users = new DataSet1TableAdapters.Users1TableAdapter();
                //users.InsertUser1(Username.Text, Email.Text, hashedPassword, "User", DateTime.Now);
                int user_Id = int.Parse(users.InsertUserAndGetId(Username.Text, Email.Text, hashedPassword, "User", DateTime.Now).ToString());


                // Insert details into Customer table
                DataSet1TableAdapters.Customer1TableAdapter customer = new DataSet1TableAdapters.Customer1TableAdapter();
                customer.InsertCustomer1(user_Id, hashedPassword, Email.Text);

                // Sign the user in
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }

        // HashPassword method to hash plaintext passwords
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower(); // Convert to hexadecimal
            }
        }
    }
}
