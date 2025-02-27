using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Checkmate.com2.Profile
{
    public partial class Addresses : System.Web.UI.Page
    {
        private string connString = ConfigurationManager.ConnectionStrings["GroupWst27Menu"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAddress();
            }

        }

        private void LoadAddress()
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
                conn.Open();

                string AddressQuery = @"SELECT addresses, City, ZIP, Province 
                FROM Addresses 
                WHERE UserID = @UserID";

                SqlCommand cmd = new SqlCommand(AddressQuery, conn);
                cmd.Parameters.AddWithValue("@UserID", parsedUserId);
                cmd.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAddress.DataSource = dt;
                gvAddress.DataBind();

            }
        }

    }
}