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
    public partial class Cart : System.Web.UI.Page
    {
        private string connString = ConfigurationManager.ConnectionStrings["GroupWst27Menu"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["cart"] == null)
                {
                    Session["cart"] = new List<ShoppingCart>();
                }

                LoadCartData();
            }
        }

        private void LoadCartData()
        {
            List<ShoppingCart> cart = (List<ShoppingCart>)Session["cart"];

            if (cart.Count == 0)
            {
                LabelCartCount.Text = "Your cart is empty.";
                GridView1.Visible = false;
            }
            else
            {
                int cartItem = cart.Sum(item => item.Quantity);
                LabelCartCount.Text = $"You have {cartItem} items in your cart.";
                GridView1.DataSource = cart;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int prodId = int.Parse(e.CommandArgument.ToString());
            List<ShoppingCart> cart = (List<ShoppingCart>)Session["cart"];

            if (e.CommandName == "Increment")
            {
                UpdateCartQuantity(prodId, 1);
                
            }
            else if (e.CommandName == "Decrement")
            {
                UpdateCartQuantity(prodId, -1);
            }
            else if (e.CommandName == "Remove")
            {
                RemoveFromCart(prodId);
            }

            LoadCartData();
        }

        private void UpdateCartQuantity(int prodId, int change)
        {
            List<ShoppingCart> cart = (List<ShoppingCart>)Session["cart"];

            if (cart != null)
            {
                var item = cart.FirstOrDefault(x => x.Prod_Id == prodId);
                if (item != null)
                {
                    item.Quantity += change;
                    if (item.Quantity < 1) item.Quantity = 1; // Prevent going below 1
                    
                    //update subtotal of an Item
                    item.SubTotal = item.Quantity * item.Price;
                }
            }

            Session["cart"] = cart;
        }

        private void RemoveFromCart(int prodId)
        {
            List<ShoppingCart> cart = (List<ShoppingCart>)Session["cart"];

            if (cart != null)
            {
                var item = cart.FirstOrDefault(x => x.Prod_Id == prodId);
                if (item != null)
                {
                    // Update product quantity back to stock in DB
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        string updateQuery = @"UPDATE Product_Item SET Prod_Quantity_Available += @Quantity WHERE Prod_ID = @ProductID";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@ProductID", prodId);
                            cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    cart.Remove(item);
                }
            }

            Session["cart"] = cart;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = e.Row.RowIndex;
                e.Row.CssClass = "cart-row remove-row-" + rowIndex;
            }
        }
    }
}


