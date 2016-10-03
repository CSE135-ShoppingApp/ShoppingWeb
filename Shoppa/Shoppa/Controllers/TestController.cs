using Shoppa.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppa.Controllers
{
    [AllowAnonymous]
    public class TestController : Controller
    {
        shoppa135dbEntities context = new shoppa135dbEntities();
        // GET: Test
        public ActionResult Index(string rowsMenu, string orderMenu, string productCategories, int rowPage = 0, int columnPage = 0)
        {
            // FILTERING OPTIONS:
            string[] rowsMenuList = new string[] { "Customers", "States" };
            // Select default and assign list
            ViewBag.RowsMenu = new SelectList(rowsMenuList, rowsMenu ?? "Customers");
            string[] orderMenuList = new string[] { "Alphabetical", "Top-K" };
            ViewBag.OrderMenu = new SelectList(orderMenuList, orderMenu ?? "Alphabetical");
            var productCategoriesList = context.categories.Select(x => x.name).ToList();
            ViewBag.ProductCategories = new SelectList(productCategoriesList, productCategories ?? "All");
            
            //string productsStr = "products"
            //// category filtering
            //if( !String.IsNullOrEmpty(productCategories) )
            //{
            //    productsStr = "(SELECT * FROM products WHERE   )";
            //}
            bool rowChoiceUsers = rowsMenu == "Customers" || String.IsNullOrEmpty(rowsMenu);
            // row selection
            string rowSelect = "users.state";
            if (rowChoiceUsers)
            {
                rowSelect = "users.id, users.name";
            }
            // order selection
            string orderSelect = "SUM(quantity*price) DESC";
            if( orderMenu == "Alphabetical" || String.IsNullOrEmpty(orderMenu))
            {
                if( rowSelect == "users.id, users.name" ) 
                {
                    orderSelect = "users.name";
                }
                else
                {
                    orderSelect = "users.state";
                }
            }
            Stats s = new Stats();

            // obtaining user/state names in top order
            Dictionary<string, Dictionary<string,string>> dict = new  Dictionary<string, Dictionary<string,string>>();
            List<string> rows = new List<string>();
            List<string> rowsId = new List<string>();
            query = "SELECT SUM(quantity*price) AS totals, " + rowSelect + " " +
                    "FROM users LEFT JOIN orders ON users.id = orders.user_id " +
                    "GROUP BY " + rowSelect + " " +
                    "ORDER BY " + orderSelect;
            using (SqlConnection con = new SqlConnection("Server=tcp:shoppa135dbserver.database.windows.net,1433;Database=shoppa135db;User ID=shoppa135dbuser@shoppa135dbserver;Password=@C5e1352016;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        int nameIdx = 2;
                        if (rowSelect == "users.state")
                        {
                            nameIdx = 1;
                        }
                        //actual processing of query result
                        while( reader.Read() )
                        {
                            string rowName = reader.GetValue(0).ToString();
                            rows.Add( reader.GetValue(nameIdx).ToString() + " (" + ( rowName == "" ? "0" : rowName )+ ")");
                            dict.Add( reader.GetValue(1).ToString(), new Dictionary<string,string>() );
                            rowsId.Add( reader.GetValue(1).ToString() );
                        }
                    }
                }
            }   // end of using sqlconnection



            // obtaining top product ids
            List<string> cols = new List<string>();
            List<string> colsId = new List<string>();
            string order = "SUM(o.quantity * o.price) DESC";
            if( orderMenu == "Alphabetical" || String.IsNullOrEmpty(orderMenu))
            {
                order = "p.name";
            }
            query = "SELECT		p.id, p.name, SUM(o.quantity * o.price) AS total " + 
                    "FROM		products p LEFT JOIN orders o ON p.id = o.product_id " +
                    "GROUP BY	p.id, p.name " + 
                    "ORDER BY " + order;
            using (SqlConnection con = new SqlConnection("Server=tcp:shoppa135dbserver.database.windows.net,1433;Database=shoppa135db;User ID=shoppa135dbuser@shoppa135dbserver;Password=@C5e1352016;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        //actual processing of query result
                        while (reader.Read())
                        {
                            string rowName = reader.GetValue(2).ToString();
                            cols.Add(reader.GetValue(1).ToString() + " (" + (rowName == "" ? "0" : rowName) + ")");
                            colsId.Add(reader.GetValue(0).ToString());
                        }
                    }
                }
            }   // end of using sqlconnection
            


            // getting the data for the user/state x product total sales
            string rowType = rowChoiceUsers ? "users.id" : "state";
            query = "SELECT " + rowType + ", product_id, SUM(quantity * price) AS total " +
                    "FROM " + "(users INNER JOIN orders ON users.id = orders.user_id) " +
                    "GROUP BY " + rowType + ", product_id";
            using (SqlConnection con = new SqlConnection("Server=tcp:shoppa135dbserver.database.windows.net,1433;Database=shoppa135db;User ID=shoppa135dbuser@shoppa135dbserver;Password=@C5e1352016;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        //actual processing of query result
                        while (reader.Read())
                        {
                            string currEntry = reader.GetValue(0).ToString();
                            if( dict.ContainsKey(currEntry))
                            {
                                dict[currEntry].Add(reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                            }
                        }
                    }
                }
            }   // end of using sqlconnection

            /* note that the constructor need be called without (). I.e., new string[rows.Count, cols.Count]() is wrong */
            string[,] data = new string[rows.Count,cols.Count];
            for (int i = 0; i < rows.Count; i++ )
            {
                
                for( int j = 0; j < cols.Count; j++ )
                {
                    // note in C# it is not data[i][j], but rather data[i,j]
                    data[i,j] = dict[rowsId[i]].ContainsKey( colsId[j] ) ? dict[rowsId[i]][ colsId[j] ] : "0";
                }
            }

            s.cols = cols;
            s.rows = rows;
            s.data = data;
            return View(s);
        }

        private string query;
    }
}