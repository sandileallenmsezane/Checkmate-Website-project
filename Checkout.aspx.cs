using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.TeamFoundation.Client;
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
    public partial class Shopping : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrderSummary();
            }
            
        }

        private void LoadOrderSummary()
        {
            // Get cart items from session
            List<ShoppingCart> cart = (List<ShoppingCart>)Session["cart"];

            if (cart != null && cart.Count > 0)
            {
                OrderItemsRepeater.DataSource = cart;
                OrderItemsRepeater.DataBind();

                // Calculate totals
                decimal subtotal = cart.Sum(item => item.SubTotal);
                decimal delivery = 5.00m; // Fixed delivery fee
                decimal total = subtotal + delivery;

                // Display totals
                litTotal.Text = total.ToString("N2");
            }
            else
            {
                Response.Redirect("Cart.aspx"); // Redirect if cart is empty
            }
        }

        //protected void btnPlaceOrder_Click(object sender, EventArgs e)
        //{
        //   UserDataSetTableAdapters.OrdersTableAdapter ordertA = new UserDataSetTableAdapters.OrdersTableAdapter();
        //UserDataSetTableAdapters.Orders1TableAdapter orderta = new UserDataSetTableAdapters.Orders1TableAdapter();
        //UserDataSetTableAdapters.OrderDetailsTableAdapter orderdta = new UserDataSetTableAdapters.OrderDetailsTableAdapter();
        //UserDataSetTableAdapters.OrderDetails1TableAdapter orderDta = new UserDataSetTableAdapters.OrderDetails1TableAdapter();

        //UserDataSetTableAdapters.PaymentInfoTableAdapter paymentTa = new UserDataSetTableAdapters.PaymentInfoTableAdapter();
        //    try
        //    {

        ////orderId = Convert.ToInt32(cmd.ExecuteScalar());
        //orderta.InsertOrder(parsedUserId, cartItem, total,(DateTime.Today).ToString());
        //orderta.GetDataBy1();
        //        ScriptManager.RegisterStartupScript(this, GetType(), "redirectScript",
        //                "setTimeout(function() { window.location.href = 'Cart.aspx}, 3000);",
        //                true);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Show error message
        //        StatusMessage.Text = "An error occurred while processing your order. Please try again.";
        //        StatusMessage.CssClass = "status-message error-message";

        //        // You might want to log the actual error somewhere for debugging
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //    }
        //}

        protected void btnPay_Click(object sender, EventArgs e)
        {
            //Creating tableAdapters Oject
            
            try
            {
                

                string userId = Session["UserID"].ToString();
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
                {
                    // Handle missing or invalid user ID
                    Console.WriteLine("Invalid User ID.");
                    return;
                }
                // Get cart items from session
                List<ShoppingCart> cart = (List<ShoppingCart>)Session["cart"];

                if (cart != null && cart.Count > 0)
                {
                    // Calculate totals
                    decimal subtotal = cart.Sum(item => item.SubTotal);
                    int cartItem = cart.Sum(item => item.Quantity);
                    decimal delivery = 5.00m;
                    decimal total = subtotal + delivery;

                    // Get form data
                    string fullName = txtFirstName.Text + txtLastName.Text;
                    string email = txtEmail.Text;
                    string address = txtAddress.Text;
                    string city = txtCity.Text;
                    string postalCode = txtPostalCode.Text;
                    string provinces = txtProvince.Text;

                    int orderId = 0;
                    

                    // Create connection and command
                    using (SqlConnection conn = new SqlConnection("Data Source=146.230.177.46;Initial Catalog=GroupWst27;Persist Security Info=True;User ID=GroupWst27;Password=mhfd5"))
                    {

                        conn.Open();
                        /* below this code checks if the the address exist in the database and if it does 
                         * the insert query is not executed but if it does the query execute
                        */
                        // Check if address already exists
                        string checkQuery = @"SELECT COUNT(*) FROM Addresses WHERE Fullname = @Fullname AND Email = @Email AND addresses = @addresses";

                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@Fullname", fullName);
                            checkCmd.Parameters.AddWithValue("@Email", email);
                            checkCmd.Parameters.AddWithValue("@addresses", address);

                            int addressCount = (int)checkCmd.ExecuteScalar();

                            if (addressCount == 0)
                            {
                                // Address doesn't exist, proceed with the insert
                                string addressQuery = @"INSERT INTO Addresses 
                                    (UserID, Fullname, Email, addresses, City, ZIP, Province)
                                    VALUES (@UserID, @Fullname, @Email, @addresses, @City, @ZIP, @Province)";

                                using (SqlCommand acmd = new SqlCommand(addressQuery, conn))
                                {
                                    acmd.Parameters.AddWithValue("@UserID", parsedUserId);
                                    acmd.Parameters.AddWithValue("@Fullname", fullName);
                                    acmd.Parameters.AddWithValue("@Email", email);
                                    acmd.Parameters.AddWithValue("@addresses", address);
                                    acmd.Parameters.AddWithValue("@City", city);
                                    acmd.Parameters.AddWithValue("@ZIP", postalCode);
                                    acmd.Parameters.AddWithValue("@Province", provinces);
                                    acmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Address already exists, do not insert
                                Console.WriteLine("This address already exists in the database.");
                            }
                        }
                        /*
                         *Insert query inserting into the orders - overall order in session
                         *Inserting into orderDetails the product and quantity bought per product 
                        */
                        string orderQuery = @"INSERT INTO Orders
                        (UserID, Quantity, TotalAmount, OrderDate)
                        VALUES (@UserID,@Quantity,@TotalAmount,@OrderDate);
                        SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(orderQuery, conn))
                        {

                            cmd.Parameters.AddWithValue("@UserID", parsedUserId);
                            cmd.Parameters.AddWithValue("@Quantity", cartItem);
                            cmd.Parameters.AddWithValue("@TotalAmount",total);
                            cmd.Parameters.AddWithValue("@OrderDate", DateTime.Today);

                            // Get the newly created order ID
                            orderId = Convert.ToInt32(cmd.ExecuteScalar());

                            // Now insert order details
                            string detailsQuery = @"INSERT INTO OrderDetails 
                        (OrderID, ProductID, Quantity, Price, Subtotal) 
                        VALUES (@orderId, @productId, @quantity, @price, @subtotal)";

                            foreach (var item in cart)
                            {
                                using (SqlCommand detailCmd = new SqlCommand(detailsQuery, conn))
                                {
                                    detailCmd.Parameters.AddWithValue("@orderId", orderId);
                                    detailCmd.Parameters.AddWithValue("@productId", item.Prod_Id);
                                    detailCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                                    detailCmd.Parameters.AddWithValue("@price", item.Price);
                                    detailCmd.Parameters.AddWithValue("@subtotal", item.SubTotal);
                                    detailCmd.ExecuteNonQuery();
                                }
                            }

                            /*We check first if the PaymentInfo table contain the address being insert 
                             * if it does the insert query does not execute
                             * else the query execute
                            */
                            string checkInfo = @"SELECT COUNT(*) FROM PaymentInfo WHERE CardName = @cardName AND CardNumber = @cardNum AND SecurityCode = @securityCode";

                            using (SqlCommand checkinfocmd = new SqlCommand(checkInfo, conn))
                            {
                                checkinfocmd.Parameters.AddWithValue("@cardName", txtName.Text);
                                checkinfocmd.Parameters.AddWithValue("@cardNum", txtCardNumber.Text);
                                checkinfocmd.Parameters.AddWithValue("@securityCode", txtCVC.Text);

                                int InfoCount = (int)checkinfocmd.ExecuteScalar();

                                if(InfoCount == 0)
                                {
                                    //Inserting payment info to PaymentInfo table
                                    string paymentQuery = @"INSERT INTO PaymentInfo
                                    (UserID, CardName, CardNumber, Expiry, SecurityCode, PaymentDate)
                                    VALUES (@userId, @cardname, @cardnum, @expiry, @securitycode, @paymentdate)";

                                    using (SqlCommand paymentCmd = new SqlCommand(paymentQuery, conn))
                                    {
                                        paymentCmd.Parameters.AddWithValue("@userId", Convert.ToInt32(userId));
                                        paymentCmd.Parameters.AddWithValue("@cardName", txtName.Text.Trim());
                                        paymentCmd.Parameters.AddWithValue("@cardnum", txtCardNumber.Text.Trim());
                                        paymentCmd.Parameters.AddWithValue("@expiry", txtExpiry.Text.Trim());
                                        paymentCmd.Parameters.AddWithValue("@securitycode", txtCVC.Text.Trim());
                                        paymentCmd.Parameters.AddWithValue("@paymentdate", DateTime.Now);
                                        paymentCmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("This card already exists in the database.");
                                }
                            }

                            
                        }


                    }

                    // Clear the cart
                    Session["cart"] = null;

                    // Show success message
                    StatusMessage.Text = "Order placed successfully! Your order number is: " + orderId;
                    StatusMessage.CssClass = "status-message success-message";

                    //// Optional: Disable the place order button
                    btnPay.Enabled = false;

                    ScriptManager.RegisterStartupScript(this, GetType(), "redirectScript",
                        "setTimeout(function() { window.location.href = 'OrderHistory.aspx}, 3000);",
                        true);
                }
                else
                {
                    StatusMessage.Text = "Your cart is empty!";
                    StatusMessage.CssClass = "status-message error-message";
                }
            }
            catch (Exception ex)
            {
                // Display error message in the UI
                StatusMessage.Text = "An error occurred: " + ex.Message;
                StatusMessage.CssClass = "status-message error-message";

                // You might want to log the actual error somewhere for debugging
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected void PaymentMethodChanged(object sender, EventArgs e)
        {
            pnlCardDetails.Visible = rbCard.Checked;
        }

    }
}