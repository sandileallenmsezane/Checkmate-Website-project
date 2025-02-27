using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Checkmate.com2
{
    public partial class Shop : System.Web.UI.Page
    {
        List<ShoppingCart> cart = new List<ShoppingCart>();
        private string connString = ConfigurationManager.ConnectionStrings["GroupWst27Menu"].ConnectionString;
        private const int PageSize = 9; // Define the page size (adjust as needed)

        // Keeps track of the current page index
        private int PageIndex
        {
            get { return (int)(ViewState["PageIndex"] ?? 0); }
            set { ViewState["PageIndex"] = value; }
        }


        private string SelectedCategory
        {
            get { return ViewState["SelectedCategory"] as string ?? "All"; }
            set { ViewState["SelectedCategory"] = value; }
        }


        //private int PageSize = 9; // Number of items per page (3x3 grid)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();  // Ensure categories load once on initial page load
                BindData();  // Load the products list


                if (Session["cart"] == null)
                {
                    Session["cart"] = new List<ShoppingCart>();
                }
                cart = (List<ShoppingCart>)Session["cart"];
                LabelCartCount.Text = cart.Count.ToString();
            }
           

        }

        private int GetTotalRowCount()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT COUNT(*) FROM Product_Item WHERE (@Category = 'All' OR Category = @Category)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Category", SelectedCategory);
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
            //int totalRows = 0;
            //using (SqlConnection conn = new SqlConnection("Data Source=146.230.177.46;Initial Catalog=GroupWst27;Persist Security Info=True;User ID=GroupWst27;Password=mhfd5"))
            //{
            //    string query = "SELECT COUNT(*) FROM Product_Item";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    conn.Open();
            //    totalRows = (int)cmd.ExecuteScalar();
            //}
            //return totalRows;

        }

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT DISTINCT Category FROM Product_Item";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Debug: Check if the Category column is present
                if (dt.Columns.Contains("Category"))
                {
                    CategoryRepeater.DataSource = dt;
                    CategoryRepeater.DataBind();
                }
                else
                {
                    Response.Write("<script>alert('Error: 'Category' column not found in dataset.');</script>");
                   
                }
            }
        }

        private void BindCategories()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT DISTINCT Category AS CategoryName FROM Product_Item";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    CategoryRepeater.DataSource = dt;
                    CategoryRepeater.DataBind();
                }
            }

        }

        // Binds data for the current page
        private void BindData()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT Prod_ID, Prod_Description, ImageUrl, Prod_Price, Prod_Quantity_Available 
                         FROM Product_Item 
                         WHERE (@Category IS NULL OR Category = @Category)
                         ORDER BY Prod_ID 
                         OFFSET @StartRow ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // If no category is selected, use NULL (to retrieve all products)
                    cmd.Parameters.AddWithValue("@Category", string.IsNullOrEmpty(SelectedCategory) || SelectedCategory == "All" ? (object)DBNull.Value : SelectedCategory);
                    cmd.Parameters.AddWithValue("@StartRow", PageIndex * PageSize);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    DataList1.DataSource = dt;
                    DataList1.DataBind();
                }
            }

            UpdatePagerButtons(); // Update navigation buttons
        }


        // Handles the Previous button click
        protected void PreviousPage_Click(object sender, EventArgs e)
        {
            if (PageIndex > 0)  // Prevents negative values
            {
                PageIndex--;  // Decrement PageIndex
                LoadProducts();
            }
        }

        protected void NextPage_Click(object sender, EventArgs e)
        {
            PageIndex++;  // Increment PageIndex
            LoadProducts();
        }

        private void LoadProducts()
        {
            if (ViewState["SelectedCategory"] != null)
            {
                FilterProductsByCategory(ViewState["SelectedCategory"].ToString());
            }
            else
            {
                BindData();
            }
        }


        // Updates the visibility and state of pager buttons
        private void UpdatePagerButtons()
        {
            int totalRows = GetTotalRowCount();
            int maxPageIndex = (totalRows + PageSize - 1) / PageSize - 1;

            PreviousButton.Enabled = PageIndex > 0;
            NextButton.Enabled = PageIndex < maxPageIndex;
        }

        protected void CategoryRadio_CheckedChanged(object sender, EventArgs e)
        {

            // Uncheck all radio buttons in the Repeater
            foreach (RepeaterItem item in CategoryRepeater.Items)
            {
                RadioButton rb = (RadioButton)item.FindControl("CategoryRadio");
                if (rb != null)
                {
                    rb.Checked = false;
                }
            }

            // Set only the clicked radio button as checked
            RadioButton selectedRadio = (RadioButton)sender;
            selectedRadio.Checked = true;

            // Get the selected category and filter products
            string selectedCategory = selectedRadio.Text;
            FilterProductsByCategory(selectedCategory);
        }

        private void FilterProductsByCategory(string category)
        {
            int pageSize = 9; // Adjust this based on your pagination size
            int pageIndex = 1; // Default to the first page

            if (ViewState["PageIndex"] != null)
            {
                pageIndex = (int)ViewState["PageIndex"];
            }

            int startRow = (pageIndex - 1) * pageSize + 1;
            int endRow = pageIndex * pageSize;

            //Response.Write("<script>alert('Filtering products for category: " + category + "');</script>");

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GroupWst27Menu"].ConnectionString))
            {
                conn.Open();
                string query = @"
                SELECT * FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY Prod_ID) AS RowNum,
                    [Prod_ID], [ImageUrl], [Prod_Description], [Prod_Price]
                    FROM [Product_Item]
                    WHERE Category = @Category
                ) AS T 
                WHERE RowNum BETWEEN @StartRow AND @EndRow";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@StartRow", startRow);  // Adjust this for pagination
                cmd.Parameters.AddWithValue("@EndRow", endRow);    // Adjust this for pagination

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataList1.DataSource = dt;
                    DataList1.DataBind();

                    //Response.Write("<script>alert('Products loaded successfully!');</script>");
                }
                else
                {
                    //Response.Write("<script>alert('No products found for this category');</script>");
                    DataList1.DataSource = null;
                    DataList1.DataBind();

                }
            }

        }
        protected void CategoryRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "FilterCategory")
            {
                string selectedCategory = e.CommandArgument.ToString();
                ViewState["SelectedCategory"] = selectedCategory;  //  Store the selected category
                ViewState["PageIndex"] = 0;  //  Reset pagination on category change
                FilterProductsByCategory(selectedCategory);
            }
        }

        protected void AllCategoriesButton_Click(object sender, EventArgs e)
        {
            SelectedCategory = null; // Set category to null or empty to fetch all products
            PageIndex = 0; // Reset pagination
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                SqlDataSource1.SelectCommand = @"
            SELECT * FROM (
                SELECT ROW_NUMBER() OVER (ORDER BY Prod_ID) AS RowNum,
                [Prod_ID], [ImageUrl], [Prod_Description], [Prod_Price]
                FROM [Product_Item]
                WHERE Prod_Description LIKE @SearchTerm
            ) AS T 
            WHERE RowNum BETWEEN @StartRow AND @EndRow";

                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("SearchTerm", "%" + searchTerm + "%");
                SqlDataSource1.SelectParameters.Add("StartRow", TypeCode.Int32, "1");
                SqlDataSource1.SelectParameters.Add("EndRow", TypeCode.Int32, "9");
            }
            else
            {
                // Reset to original query if search box is empty
                SqlDataSource1.SelectCommand = @"
            SELECT * FROM (
                SELECT ROW_NUMBER() OVER (ORDER BY Prod_ID) AS RowNum,
                [Prod_ID], [ImageUrl], [Prod_Description], [Prod_Price]
                FROM [Product_Item]
            ) AS T 
            WHERE RowNum BETWEEN @StartRow AND @EndRow";

                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("StartRow", TypeCode.Int32, "1");
                SqlDataSource1.SelectParameters.Add("EndRow", TypeCode.Int32, "9");
            }
            SqlDataSource1.DataBind();
            DataList1.DataBind();
            BindCategories();
        }




        // Product class to represent cart items
        public class Product
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton clickedButton = (ImageButton)sender;
                string itemID = clickedButton.CommandArgument;

                // Check product quantity before proceeding
                if (!CheckProductQuantity(itemID))
                {
                    LabelCartCount.Text = $"Sorry, this item is Sold Out.";
                    return;
                }

                // First update the product quantity in inventory
                UpdateProductQuantity(itemID);

                // Then handle the shopping cart update
                AddToShoppingCart(itemID);
            }
            catch (Exception ex)
            {
                LabelCartCount.Text = "Error processing request: " + ex.Message;
            }
        }

        private bool CheckProductQuantity(string itemID)
        {
            if (string.IsNullOrEmpty(itemID))
            {
                throw new ArgumentNullException(nameof(itemID));
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string checkQuery = @"SELECT Prod_Quantity_Available 
                                FROM Product_Item 
                                WHERE Prod_ID = @ProductID";

                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", int.Parse(itemID));

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int currentQuantity = Convert.ToInt32(result);
                            return currentQuantity > 2; // Return true only if quantity is above threshold
                        }
                        return false; // Product not found
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database error occurred while checking quantity: " + ex.Message);
                }
            }
        }
        private void AddToShoppingCart(string itemID)
        {
            try
            {
                // Input validation
                if (string.IsNullOrEmpty(itemID))
                {
                    throw new ArgumentNullException(nameof(itemID));
                }

                // Retrieve the existing cart from the session
                List<ShoppingCart> cart = (List<ShoppingCart>)Session["cart"] ?? new List<ShoppingCart>();

                // Get product details
                DataSet1 DS = new DataSet1();
                DataSet1TableAdapters.Product_ItemTableAdapter TA = new DataSet1TableAdapters.Product_ItemTableAdapter();
                TA.FillBy(DS.Product_Item, int.Parse(itemID));

                if (DS.Product_Item.Rows.Count == 0)
                {
                    LabelCartCount.Text = "Item not found.";
                    return;
                }

                string enteredValue = "1";  // Default value Of quantity
                ShoppingCart newITEM = new ShoppingCart
                {
                    Prod_Id = int.Parse(itemID),
                    Prod_Name = DS.Product_Item.Rows[0][1].ToString(),
                    Quantity = int.Parse(enteredValue),
                    Price = Decimal.Parse(DS.Product_Item.Rows[0][2].ToString()),
                    SubTotal = Decimal.Parse(enteredValue) * Decimal.Parse(DS.Product_Item.Rows[0][2].ToString())
                };

                // Check if the item already exists in the cart
                bool itemExists = false;
                foreach (var cartItem in cart)
                {
                    if (cartItem.Prod_Id == newITEM.Prod_Id)
                    {
                        cartItem.Quantity += newITEM.Quantity;
                        cartItem.SubTotal += newITEM.SubTotal;
                        itemExists = true;
                        break;
                    }
                }

                // If the item does not exist, add it to the cart
                if (!itemExists)
                {
                    cart.Add(newITEM);
                }

                // Save the cart back to the session
                Session["cart"] = cart;

                // Update cart count display
                LabelCartCount.Text = $"Cart contains: {cart.Count} items.";
            }
            catch (FormatException ex)
            {
                throw new Exception("Error processing item data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding item to cart: " + ex.Message);
            }
        }

        public void UpdateProductQuantity(string itemId)
        {
            // Validate input
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException(nameof(itemId), "Item ID cannot be null or empty");
            }

            if (!int.TryParse(itemId, out int productId))
            {
                throw new ArgumentException("Item ID must be a valid integer", nameof(itemId));
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Update product quantity query
                    string updateQuery = @"UPDATE Product_Item 
                                 SET Prod_Quantity_Available -= @Quantity
                                 WHERE Prod_ID = @ProductID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@Quantity", 1);

                        // Execute the update query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Quantity updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("No matching product found, quantity not updated.");
                        }
                    }
                }
                catch (SqlException ex)
                {
                    StatusMessage.Text = "An error occurred: " + ex.Message;
                    StatusMessage.CssClass = "status-message error-message";

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                   
                }
                catch (Exception ex)
                {
                    StatusMessage.Text = "An error occurred: " + ex.Message;
                    StatusMessage.CssClass = "status-message error-message";

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    
                }
            }

        }

        

        //protected void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string selectedCategory = CategoryList1.SelectedValue;
        //    // Debugging: Check if the selected category is "Drinks"
        //    if (selectedCategory == "Drinks")
        //    {
        //        // You can log it to debug output
        //        System.Diagnostics.Debug.WriteLine("Selected Category is: Drinks");
        //        // Or, display it on the page (add a label control for this)
        //        LabelCartCount.Text = "Category selected: " + selectedCategory;
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine("Selected Category: " + selectedCategory);
        //    }
        //    // Set pagination values dynamically (assuming PageIndex starts from 1)
        //    int pageIndex = 1; // You need to dynamically get the current page index
        //    int pageSize = 9;  // Set how many items per page
        //    int startRow = ((pageIndex - 1) * pageSize) + 1;
        //    int endRow = pageIndex * pageSize;

        //    // Update the SQL command to include pagination with the category filter
        //    SqlDataSource1.SelectCommand = @"
        //    SELECT * FROM (
        //        SELECT ROW_NUMBER() OVER (ORDER BY Prod_ID) AS RowNum,
        //               [Prod_ID], [ImageUrl], [Prod_Description], [Prod_Price]
        //        FROM [Product_Item]
        //        WHERE Category = @Category
        //    ) AS T 
        //    WHERE RowNum BETWEEN @StartRow AND @EndRow";

        //    // Clear existing parameters to avoid duplicates
        //    SqlDataSource1.SelectParameters.Clear();

        //    // Add parameters correctly
        //    SqlDataSource1.SelectParameters.Add("Category", DbType.String, "Drinks");
        //    SqlDataSource1.SelectParameters.Add("StartRow", DbType.Int32, startRow.ToString());
        //    SqlDataSource1.SelectParameters.Add("EndRow", DbType.Int32, endRow.ToString());

        //    // Rebind the DataList
        //    SqlDataSource1.DataBind();
        //    DataList1.DataBind();

        //    // If you need to update other UI elements or counts
        //    BindData();
        //}


        //protected void CategoryList_Click(object sender, BulletedListEventArgs e)
        //{
        //    string selectedCategory = CategoryList1.SelectedValue;
        //    // Debugging: Check if the selected category is "Drinks"
        //    if (selectedCategory == "Drinks")
        //    {
        //        // You can log it to debug output
        //        System.Diagnostics.Debug.WriteLine("Selected Category is: Drinks");
        //        // Or, display it on the page (add a label control for this)
        //        LabelCartCount.Text = "Category selected: " + selectedCategory;
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine("Selected Category: " + selectedCategory);
        //    }



        //    // Set pagination values dynamically (assuming PageIndex starts from 1)
        //    int pageIndex = 1; // You need to dynamically get the current page index
        //    int pageSize = 9;  // Set how many items per page
        //    int startRow = ((pageIndex - 1) * pageSize) + 1;
        //    int endRow = pageIndex * pageSize;

        //    // Update the SQL command to include pagination with the category filter
        //    SqlDataSource1.SelectCommand = @"
        //    SELECT * FROM (
        //        SELECT ROW_NUMBER() OVER (ORDER BY Prod_ID) AS RowNum,
        //               [Prod_ID], [ImageUrl], [Prod_Description], [Prod_Price]
        //        FROM [Product_Item]
        //        WHERE Category = @Category
        //    ) AS T 
        //    WHERE RowNum BETWEEN @StartRow AND @EndRow";

        //    // Clear existing parameters to avoid duplicates
        //    SqlDataSource1.SelectParameters.Clear();

        //    // Add parameters correctly
        //    SqlDataSource1.SelectParameters.Add("Category", DbType.String, "Drinks");
        //    SqlDataSource1.SelectParameters.Add("StartRow", DbType.Int32, startRow.ToString());
        //    SqlDataSource1.SelectParameters.Add("EndRow", DbType.Int32, endRow.ToString());

        //    // Rebind the DataList
        //    SqlDataSource1.DataBind();
        //    DataList1.DataBind();

        //    // If you need to update other UI elements or counts
        //    BindData();
        //}
        

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //// Check if "AddtoCart" button is clicked
            //if (e.CommandName == "AddtoCart")
            //{
            //    // Retrieve product details
            //    string productName = e.CommandArgument.ToString();
            //    string productPrice = ((Label)e.Item.FindControl("ProductPriceLabel")).Text;
            //    string productImageUrl = ((Image)e.Item.FindControl("ProductImage")).ImageUrl;

            //    // Create product object
            //    Product product = new Product
            //    {
            //        Name = productName,
            //        Price = Convert.ToDecimal(productPrice),
            //        ImageUrl = productImageUrl
            //    };

            //    // Retrieve existing cart from session or initialize a new one
            //    List<Product> cart = Session["Cart"] as List<Product>;
            //    if (cart == null)
            //    {
            //        cart = new List<Product>();
            //    }

            //    // Add the product to the cart
            //    cart.Add(product);

            //    // Update session
            //    Session["Cart"] = cart;

            //    // Redirect to Cart page (optional)
            //    Response.Redirect("Profile/Cart.aspx");
            //}

        }


        private void AddToCart_Click(object sender, ImageClickEventArgs e)
        {
            //ImageButton clickedButton = (ImageButton)sender;
            //string itemID = clickedButton.CommandArgument;

            //// Retrieve the existing cart from the session
            //List<ShoppingCart> cart = (List<ShoppingCart>)Session["cart"];
            //if (cart == null) // Initialize if null
            //{
            //    cart = new List<ShoppingCart>();
            //}

            //// Get product details
            //DataSet1 DS = new DataSet1();
            //DataSet1TableAdapters.Product_ItemTableAdapter TA = new DataSet1TableAdapters.Product_ItemTableAdapter();
            //TA.FillBy(DS.Product_Item, int.Parse(itemID));

            //if (DS.Product_Item.Rows.Count == 0)
            //{
            //    // Handle case where the product is not found
            //    LabelCartCount.Text = "Item not found.";
            //    return;
            //}

            //DataListItem item = (DataListItem)clickedButton.NamingContainer;
            //DropDownList myDropDownList = (DropDownList)item.FindControl("DropDownList1");
            //string selectedValue = "1";

            //if (myDropDownList != null)
            //{
            //    selectedValue = myDropDownList.SelectedValue;
            //}

            //ShoppingCart newITEM = new ShoppingCart
            //{
            //    Prod_Id = int.Parse(itemID),
            //    Prod_Name = DS.Product_Item.Rows[0][1].ToString(),
            //    Quantity = int.Parse(selectedValue),
            //    Price = Decimal.Parse(DS.Product_Item.Rows[0][2].ToString()),
            //    SubTotal = Decimal.Parse(selectedValue) * Decimal.Parse(DS.Product_Item.Rows[0][2].ToString())
            //};

            //// Check if the item already exists in the cart
            //bool itemExists = false;
            //foreach (var cartItem in cart)
            //{
            //    if (cartItem.Prod_Id == newITEM.Prod_Id)
            //    {
            //        cartItem.Quantity += newITEM.Quantity;
            //        cartItem.SubTotal += newITEM.SubTotal;
            //        itemExists = true;
            //        break;
            //    }
            //}

            //// If the item does not exist, add it to the cart
            //if (!itemExists)
            //{
            //    cart.Add(newITEM);
            //}

            //// Save the cart back to the session
            //Session["cart"] = cart;

            //// Update cart count display
            //LabelCartCount.Text = $"Cart contains: {cart.Count} items.";
            // Label5.Text = selectedValue;
        }

        protected void DataList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}

/*
 *  ImageButton clickedButton = (ImageButton)sender;

            // Get the value of the CommandArgument, which is the item ID
            string itemID = clickedButton.CommandArgument;
            DataSet1 DS = new DataSet1();
            DataSet1TableAdapters.Product_ItemTableAdapter TA = new DataSet1TableAdapters.Product_ItemTableAdapter();
            TA.FillBy(DS.Product_Item, int.Parse(itemID));
            DataListItem item = (DataListItem)clickedButton.NamingContainer;
            DropDownList myDropDownList = (DropDownList)item.FindControl("DropDownList1");
            string selectedValue = "1";
            // Now, you can use myDropDownList as needed
            if (myDropDownList != null)
            {
                // Access properties or perform actions on the DropDownList
                 selectedValue = myDropDownList.SelectedValue;
                // Your logic here...
            }
            else
            {

            }
            ShoppingCart newITEM = new ShoppingCart
            {
                Prod_Id = int.Parse(itemID),


                Prod_Name = DS.Product_Item.Rows[0][1].ToString(),

                Quantity = int.Parse(selectedValue),
            Price =Decimal.Parse(DS.Product_Item.Rows[0][2].ToString()),

              SubTotal= Decimal.Parse(selectedValue)* Decimal.Parse(DS.Product_Item.Rows[0][2].ToString())
            };

            int count = 0;
            for(int i=0; i < cart.Count; i++)
            {
                if (cart[i].Prod_Id== newITEM.Prod_Id)
                {
                    cart[i].Quantity += newITEM.Quantity;
                    count++;

                }
            }
            if (count == 0)
            {
                cart.Add(newITEM);
            }
           
            Session["cart"] = cart;
            LabelCartCount.Text = cart.Count.ToString();
           // Label5.Text = selectedValue;

            
 *
 */



/*
 *ImageButton clickedButton = (ImageButton)sender;
            string itemID = clickedButton.CommandArgument;

            DataSet1 DS = new DataSet1();
            DataSet1TableAdapters.Product_ItemTableAdapter TA = new DataSet1TableAdapters.Product_ItemTableAdapter();
            TA.FillBy(DS.Product_Item, int.Parse(itemID));

            if (DS.Product_Item.Rows.Count == 0)
            {
                LabelCartCount.Text = "Item not found.";
                return;
            }

            DataListItem item = (DataListItem)clickedButton.NamingContainer;
            DropDownList myDropDownList = (DropDownList)item.FindControl("DropDownList1");
            string selectedValue = "1";

            if (myDropDownList != null)
            {
                selectedValue = myDropDownList.SelectedValue;
            }

            ShoppingCart newITEM = new ShoppingCart
            {
                Prod_Id = int.Parse(itemID),
                Prod_Name = DS.Product_Item.Rows[0][1].ToString(),
                Quantity = int.Parse(selectedValue),
                Price = Decimal.Parse(DS.Product_Item.Rows[0][2].ToString()),
                SubTotal = Decimal.Parse(selectedValue) * Decimal.Parse(DS.Product_Item.Rows[0][2].ToString())
            };

            int count = 0;
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Prod_Id == newITEM.Prod_Id)
                {
                    cart[i].Quantity += newITEM.Quantity;
                    count++;
                    break;
                }
            }

            if (count == 0)
            {
                cart.Add(newITEM);
            }

            Session["cart"] = cart;
            LabelCartCount.Text = $"Cart updated: {cart.Count} items."; 
 */