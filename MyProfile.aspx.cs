using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;

namespace Checkmate.com2.Profile
{
    public partial class MyProfile : System.Web.UI.Page
    {
        private string connString = ConfigurationManager.ConnectionStrings["GroupWst27Menu"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserDetails();
            }
        }

        private void LoadUserDetails()
        {
            string userId = Session["UserID"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                Response.Redirect("~/Account/Login");
                return;
            }
            
            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = @"SELECT FirstName, LastName, Email, Phone, BirthDate 
                               FROM Users WHERE UserID = @UserID";


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                   

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            FirstName.Text = reader["FirstName"]?.ToString();
                            LastName.Text = reader["LastName"]?.ToString();
                            Email.Text = reader["Email"]?.ToString();
                            Phone.Text = reader["Phone"]?.ToString();
                            if (reader["BirthDate"] != DBNull.Value)
                            {
                                BirthDate.Text = Convert.ToDateTime(reader["BirthDate"]).ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
            }
        }

        protected void UpdatePersonalInfo_Click(object sender, EventArgs e)
        {
            string userId = Session["UserID"]?.ToString();

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
            {
                errormessage1.Text = "Invalid User ID.";
                errormessage1.Visible = true;
                return;
            }

            DateTime birthDate;
            bool isValidDate = DateTime.TryParseExact(
                BirthDate.Text.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out birthDate
            );

            if (!isValidDate)
            {
                errormessage1.Text = "Invalid birthdate format.";
                errormessage1.Visible = true;
                return;
            }

            // Update customer details
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string UpdateInfoQuery = @"UPDATE Users
                    SET FirstName = @FirstName, LastName = @LastName, Phone = @Phone, BirthDate = @BirthDate
                    WHERE UserID = @UserID";

                    using (SqlCommand ucmd = new SqlCommand(UpdateInfoQuery, conn))
                    {
                        ucmd.Parameters.AddWithValue("@FirstName", FirstName.Text);
                        ucmd.Parameters.AddWithValue("@LastName", LastName.Text);
                        ucmd.Parameters.AddWithValue("@Phone", Phone.Text);
                        ucmd.Parameters.AddWithValue("@BirthDate", birthDate);
                        ucmd.Parameters.AddWithValue("@UserID", parsedUserId);  // Ensure this is added

                        int rowsAffected = ucmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            successMessage1.Visible = true;
                            successMessage1.Text = "Details updated successfully!";
                        }
                        else
                        {
                            errormessage1.Text = "Update failed. No records changed.";
                            errormessage1.Visible = true;
                        }

                    }
                }
                catch (Exception ex)  // Catch all exceptions
                {
                    errormessage1.Text = "Error Occurred: " + ex.Message;
                    errormessage1.Visible = true;
                }
            }
        }


        protected void UpdateEmail_Click(object sender, EventArgs e)
        {
           
            string userId = Session["UserID"].ToString();
                
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
            {
                // Handle missing or invalid user ID
                Console.WriteLine("Invalid User ID.");
                return;
            }


            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string UpdateEQuery = @"UPDATE Users
                    SET     Email = @Email
                    WHERE  (UserID = @UserID)";

                    using(SqlCommand ecmd = new SqlCommand(UpdateEQuery, conn))
                    {
                        ecmd.Parameters.AddWithValue("@Email", Email.Text);
                        ecmd.Parameters.AddWithValue("@UserID", parsedUserId);

                        int rowsAffected = ecmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            successM.Visible = true;
                            successM.Text = "Email updated successfully!";
                        }
                        else
                        {
                            errorM.Text = "Update failed. No records changed.";
                            errorM.Visible = true;
                        }

                    }

                }
                catch (Exception ex)
                {
                    errorM.Text = "Ërror Occurred : " + ex.Message;
                    errorM.Visible = true;
                }
            }
  
        }

        protected void UpdatePassword_Click(object sender, EventArgs e)
        {
            try 
            {
                string userId = Session["UserID"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    errorMessage.Text = "User session expired. Please log in again.";
                    errorMessage.Visible = true;
                    return;
                }

                try
                {
                    // Retrieve the stored password hash from the database
                    string storedPasswordHash = "";

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        string selectQuery = "SELECT Password FROM Users WHERE UserID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            
                            storedPasswordHash = cmd.ExecuteScalar()?.ToString();
                        }
                    }

                    

                    //Compare stored hash with user-provided current password hash
                    string enteredPasswordHash = HashPassword(CurrentPassword.Text.Trim());
                    if (storedPasswordHash != enteredPasswordHash)
                    {
                        errorMessage.Text = "Current password is incorrect.";
                        errorMessage.Visible = true;
                        return;
                    }

                    System.Diagnostics.Debug.WriteLine("Stored Hash: " + storedPasswordHash);
                    System.Diagnostics.Debug.WriteLine("Entered Hash: " + enteredPasswordHash);
                }
                catch(SqlException ex)
                {
                    errorM1.Text = "Error retrieving password: " + ex.Message;
                    errorM1.Visible = true;
                    return;
                }


                try
                {
                    //Update Password in the Database
                    string newPasswordHash = HashPassword(NewPassword.Text.Trim());

                    using (SqlConnection conn = new SqlConnection(connString))
                    {

                        conn.Open();
                        string updateQuery = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 64).Value = newPasswordHash;
                            cmd.Parameters.AddWithValue("@UserID", userId);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                successP.Text = "Password updated successfully!";
                                successP.Visible = true;
                            }
                            else
                            {
                                errorMessage.Text = "No changes were made. Please try again.";
                                errorMessage.Visible = true;
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    errorM2.Text = "Error retrieving password: " + ex.Message;
                    errorM2.Visible = true;
                }
               
            }
            catch (Exception ex)
            {
                errorM1.Text = "Error :" + ex.Message;
            }

            
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        
    }
}