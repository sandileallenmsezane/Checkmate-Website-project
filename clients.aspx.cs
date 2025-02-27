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
    public partial class clients : System.Web.UI.Page
    {
        private string connString = "Data Source=146.230.177.46;Initial Catalog=GroupWst27;Persist Security Info=True;User ID=GroupWst27;Password=mhfd5";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers();
            }
        }

        private void LoadCustomers(string searchQuery = "")
        {
            using (SqlConnection conn = new SqlConnection("Data Source=146.230.177.46;Initial Catalog=GroupWst27;Persist Security Info=True;User ID=GroupWst27;Password=mhfd5"))
            {
                conn.Open();
                string query = "SELECT * FROM Customer";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query += " WHERE FirstName LIKE @Search OR LastName LIKE @Search OR Email LIKE @Search";
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

                    gvCustomers.DataSource = dt;
                    gvCustomers.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCustomers(txtSearch.Text);
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            LoadCustomers(txtSearch.Text);
        }

        protected void gvCustomers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCustomers.EditIndex = e.NewEditIndex;
            LoadCustomers(txtSearch.Text);
        }

        protected void gvCustomers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvCustomers.Rows[e.RowIndex];
            int customerId = Convert.ToInt32(gvCustomers.DataKeys[e.RowIndex].Values["Customer_ID"]);
            string firstName = (row.Cells[1].Controls[0] as TextBox).Text;
            string lastName = (row.Cells[2].Controls[0] as TextBox).Text;
            string email = (row.Cells[3].Controls[0] as TextBox).Text;
            string phone = (row.Cells[4].Controls[0] as TextBox).Text;
            string birth = (row.Cells[5].Controls[0] as TextBox).Text;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Customer SET FirstName=@FirstName, LastName=@LastName, Email=@Email, Contact_Number=@Phone, Birth_Date = @birthDate WHERE Customer_ID=@CustomerID", conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("birthDate", birth);
                cmd.ExecuteNonQuery();
            }

            gvCustomers.EditIndex = -1;
            LoadCustomers(txtSearch.Text);
        }

        protected void gvCustomers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCustomers.EditIndex = -1;
            LoadCustomers(txtSearch.Text);
        }

        protected void gvCustomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int customerId = Convert.ToInt32(gvCustomers.DataKeys[e.RowIndex].Values["Customer_ID"]);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE Customer_ID=@CustomerID", conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.ExecuteNonQuery();
            }

            LoadCustomers(txtSearch.Text);
        }


    }
}