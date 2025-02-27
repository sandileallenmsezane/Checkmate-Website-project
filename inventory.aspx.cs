using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Checkmate.com2.admin
{
    public partial class inventory : System.Web.UI.Page
    {
        private string connString = "Data Source=146.230.177.46;Initial Catalog=GroupWst27;Persist Security Info=True;User ID=GroupWst27;Password=mhfd5";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadInventory();
            }
        }

        private void LoadInventory(string searchQuery = "")
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Calculate the starting row for the current page
                int startRow = gvInventory.PageIndex * gvInventory.PageSize;

                string query = "SELECT * FROM Product_Item";
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query += " WHERE Prod_Description LIKE @Search OR Category LIKE @Search";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + searchQuery + "%");
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvInventory.DataSource = dt;
                    gvInventory.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadInventory(txtSearch.Text);
        }

        protected void gvInventory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the new page index
            gvInventory.PageIndex = e.NewPageIndex;
            // Reload inventory data
            LoadInventory(txtSearch.Text);
        }

        protected void gvInventory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvInventory.EditIndex = e.NewEditIndex;
            LoadInventory(txtSearch.Text);
        }

        protected void btnInsertProduct_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            decimal price = Convert.ToDecimal(txtPrice.Text);
            int quantity = Convert.ToInt32(txtQuantity.Text);
            string category = txtCategory.Text;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO Product_Item (Prod_Description, Prod_Price, Prod_Quantity_Available, Category) VALUES (@ProductName, @Price, @Quantity, @Category)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.ExecuteNonQuery();
                }
            }

            // Reload Inventory after Insert
            LoadInventory(txtSearch.Text);
        }

  

        protected void gvInventory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int productId = Convert.ToInt32(gvInventory.DataKeys[e.RowIndex].Values["Prod_ID"]);
            string productName = (gvInventory.Rows[e.RowIndex].FindControl("txtProductName") as TextBox).Text;
            decimal price = Convert.ToDecimal((gvInventory.Rows[e.RowIndex].FindControl("txtPrice") as TextBox).Text);
            int quantity = Convert.ToInt32((gvInventory.Rows[e.RowIndex].FindControl("txtQuantity") as TextBox).Text);
            string category = (gvInventory.Rows[e.RowIndex].FindControl("txtCategory") as TextBox).Text;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE Product_Item " +
                "SET Prod_Description=@ProductName, Prod_Price=@Price, Prod_Quantity_Available=@Quantity, Category=@category " +
                "WHERE Prod_ID=@ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.ExecuteNonQuery();
                }
            }

            gvInventory.EditIndex = -1; // Exit Edit Mode
            LoadInventory(txtSearch.Text);
        }

        protected void gvInventory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvInventory.EditIndex = -1;
            LoadInventory(txtSearch.Text);
        }

        protected void gvInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productId = Convert.ToInt32(gvInventory.DataKeys[e.RowIndex].Values["Prod_ID"]);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "DELETE FROM Product_Item WHERE Prod_ID=@ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadInventory(txtSearch.Text); // Reload inventory after deletion
        }



    }
}