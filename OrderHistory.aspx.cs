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
    public partial class OrderHistory : System.Web.UI.Page
    {
        private string connString = ConfigurationManager.ConnectionStrings["GroupWst27Menu"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        private void LoadOrders()
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

                string query = "SELECT OrderID, Quantity, TotalAmount, OrderDate FROM Orders WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", parsedUserId);
                cmd.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }

        protected void gvOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                int orderId = Convert.ToInt32(gvOrders.SelectedRow.Cells[0].Text);
                LoadOrderDetails(orderId);

                //visible Order details grid view 
                ScriptManager.RegisterStartupScript(this, GetType(), "showOrderDetails", "showOrderDetails();", true);
            }
            catch(Exception ex)
            {
                StatusMessage.Text = "An error occurred: " + ex.Message;
                StatusMessage.CssClass = "status-message error-message";

                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            
        }

        private void LoadOrderDetails(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT OD.ProductID, PI.Prod_Description, OD.Quantity, OD.Price, OD.Subtotal FROM OrderDetails OD " +
                               "JOIN Product_Item PI ON OD.ProductID = PI.Prod_ID " +
                               "WHERE OD.OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrderDetails.DataSource = dt;
                gvOrderDetails.DataBind();
            }
        }
    }
}