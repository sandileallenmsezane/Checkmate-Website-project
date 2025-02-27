using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Checkmate.com2.Profile
{
    public partial class Orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve the OrderID from the query string
                string orderId = Request.QueryString["OrderID"];
                if (!string.IsNullOrEmpty(orderId))
                {
                    LoadOrderDetails(Convert.ToInt32(orderId));
                }
                else
                {
                    lblMessage.Text = "Order ID is missing!";
                    lblMessage.CssClass = "error-message";
                }
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=146.230.177.46;Initial Catalog=GroupWst27;Persist Security Info=True;User ID=GroupWst27;Password=mhfd5"))
                {
                    conn.Open();

                    // Fetch order information
                    string orderQuery = @"SELECT CustomerName, Email, Phone, DeliveryAddress, City, PostalCode, OrderDate, TotalAmount, DeliveryFee, Status 
                                  FROM Orders WHERE OrderID = @orderId";

                    using (SqlCommand cmd = new SqlCommand(orderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderId", orderId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblCustomerName.Text = reader["CustomerName"].ToString();
                                lblEmail.Text = reader["Email"].ToString();
                                lblPhone.Text = reader["Phone"].ToString();
                                lblAddress.Text = reader["DeliveryAddress"].ToString();
                                lblCity.Text = reader["City"].ToString();
                                lblPostalCode.Text = reader["PostalCode"].ToString();
                                lblOrderDate.Text = Convert.ToDateTime(reader["OrderDate"]).ToString("dd/MM/yyyy");
                                lblTotalAmount.Text = $"R{reader["TotalAmount"]}";
                                lblDeliveryFee.Text = $"R{reader["DeliveryFee"]}";
                                lblStatus.Text = reader["Status"].ToString();
                                OrderId.Text = (string)reader["OrderID"];
                            }
                        }
                    }

                    // Fetch order details
                    string detailsQuery = @"SELECT p.Prod_Description, od.Quantity, od.Price, od.Subtotal 
                                            FROM OrderDetails od
                                            JOIN Product_Item p ON od.ProductID = p.Prod_ID
                                            WHERE od.OrderID = @orderId";

                    using (SqlCommand detailsCmd = new SqlCommand(detailsQuery, conn))
                    {
                        detailsCmd.Parameters.AddWithValue("@orderId", orderId);
                        using (SqlDataReader detailsReader = detailsCmd.ExecuteReader())
                        {
                            if (detailsReader.HasRows) // 
                            {
                           
                                DataTable dt = new DataTable();
                                dt.Load(detailsReader);
                                gvOrderDetails.DataSource = dt;
                                gvOrderDetails.DataBind();

                                System.Diagnostics.Debug.WriteLine("GridView Bound: " + gvOrderDetails.Rows.Count);
                            }
                            else
                            {

                                lblMessage.Text = "No items found for this order.";
                                lblMessage.CssClass = "error-message";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred while fetching the order details.";
                lblMessage.CssClass = "error-message";
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

    }
}