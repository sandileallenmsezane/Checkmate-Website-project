using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace Checkmate.com2.admin
{
    public partial class defaultadmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=146.230.177.46;Initial Catalog=GroupWst27;Persist Security Info=True;User ID=GroupWst27;Password=mhfd5"))
            {
                conn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Users", conn);
                lblTotalCustomers.Text = cmd1.ExecuteScalar().ToString();

                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Product_Item", conn);
                lblTotalProducts.Text = cmd2.ExecuteScalar().ToString();
            }
        }
    }
}