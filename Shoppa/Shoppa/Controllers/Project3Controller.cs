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
    public class Project3Controller : Controller
    {
        private string query, query2, query3;
        shoppa135dbEntities context = new shoppa135dbEntities();

        // GET: Project3
        
        public ActionResult Index()
        {
            var productCategoriesList = context.categories.Select(XmlSiteMapProvider => XmlSiteMapProvider.name).ToList();
            ViewBag.ProductCategories = new SelectList(productCategoriesList, "All");
            return View();
        }


        // GET: Project3
        public ActionResult Load(string productCategories)
        {
            var productCategoriesList = context.categories.Select(XmlSiteMapProvider => XmlSiteMapProvider.name).ToList();
            ViewBag.ProductCategories = new SelectList(productCategoriesList, productCategories ?? "All");

            // process all unprocessedOrders
            query = "SELECT * FROM lastProcessedOrder";
            int lastProcessedOrderId = 0;
            int count = 0;
            using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lastProcessedOrderId = Int32.Parse(reader.GetValue(0).ToString());
                        }
                        
                    }
                    con.Close();

                }

                // obtain all orders that have the id > lastProcessedOrderId
                query = "SELECT * FROM (select orders.*, users.state from orders, users where user_id = users.id) orders WHERE id > " + lastProcessedOrderId;
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while( reader.Read() )
                        {
                            // process current order
                            
                            // update product sales
                            // get current sales for product
                            double currentSales = 0;
                            string innerQuery = "select * from productSales where id = " + reader.GetValue(2).ToString();
                            using (SqlConnection con2 = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;") )
                            {
                                using (SqlCommand cmd2 = new SqlCommand(innerQuery, con2))
                                {
                                    con2.Open();
                                    using (var reader2 = cmd2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            currentSales = reader2.GetDouble(1);
                                        }
                                    }
                                }
                            }
                            // add to it
                            currentSales += reader.GetInt32(3) * reader.GetDouble(4);
                            // update current sales for product
                            innerQuery = "UPDATE productSales SET Total = '" + currentSales + "' WHERE id = " + reader.GetValue(2).ToString();
                            using (SqlConnection con2 = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
                            {
                                using (SqlCommand cmd2 = new SqlCommand(innerQuery, con2))
                                {
                                    con2.Open();
                                }
                            }



                            // update state sales
                            // get current state sales
                            double currentProductStateSales = 0;
                            string productStateInnerQuery = "select * from productStateSales where product_id = " + reader.GetValue(2).ToString() + " AND state = '" + reader.GetValue(6) + "'";
                            using (SqlConnection con2 = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
                            {
                                using (SqlCommand cmd2 = new SqlCommand(productStateInnerQuery, con2))
                                {
                                    con2.Open();
                                    using (var reader2 = cmd2.ExecuteReader())
                                    {
                                        while(reader2.Read())
                                        {
                                            currentProductStateSales = reader2.GetDouble(2);
                                        }
                                    }
                                }
                            }
                            // add to it
                            currentProductStateSales += reader.GetInt32(3) * reader.GetDouble(4);
                            // update current sales for product
                            innerQuery = "UPDATE productStateSales SET Total = '" + currentProductStateSales + "' WHERE product_id = " + reader.GetValue(0).ToString() + " AND state = '" + reader.GetValue(6) + "'";
                            using (SqlConnection con2 = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
                            {
                                using (SqlCommand cmd2 = new SqlCommand(innerQuery, con2))
                                {
                                    con2.Open();
                                }
                            }



                            // update stateSales
                            // get current state sales
                            double currentStateSales = 0;
                            string stateInnerQuery = "select * from stateSales where state = '" + reader.GetValue(6) + "'";
                            using (SqlConnection con2 = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
                            {
                                using (SqlCommand cmd2 = new SqlCommand(stateInnerQuery, con2))
                                {
                                    con2.Open();
                                    using (var reader2 = cmd2.ExecuteReader())
                                    {
                                        while( reader2.Read() ) 
                                        {
                                            currentStateSales = reader2.GetDouble(1);
                                        }
                                    }
                                }
                            }
                            // add to it
                            currentStateSales += reader.GetInt32(3) * reader.GetDouble(4);
                            // update current sales for product
                            innerQuery = "UPDATE stateSales SET Total = '" + currentStateSales + "' WHERE state = '" + reader.GetValue(6) + "'";
                            using (SqlConnection con2 = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
                            {
                                using (SqlCommand cmd2 = new SqlCommand(innerQuery, con2))
                                {
                                    con2.Open();
                                }
                            }
                            count++;
                        }

                    }

                }

            }   // end of using sqlconnection
            // if there were orders added, change the last processed order id
            if (count > 0)
            {
                string sqlQuery = "delete from lastProcessedOrder;";
                using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        con.Open();
                    }
                }   // end of using sqlconnection

                //get the last order id
                string getMax = "select max(id) from orders";
                int max= 0;
                using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
                {
                    using (SqlCommand cmd = new SqlCommand(getMax, con))
                    {
                        con.Open();
                        using(var reader = cmd.ExecuteReader() )
                        {
                            while( reader.Read() )
                            {
                                max = reader.GetInt32(0);
                            }
                        }
                    }
                }   // end of using sqlconnection
                
                string sqlUpdateQuery = "insert into lastProcessedOrder values("+ max + ");";
                using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlUpdateQuery, con))
                    {
                        con.Open();
                    }
                }   // end of using sqlconnection

            }

            // obtain state sales in top order
            Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
            List<string> rows = new List<string>();
            List<string> rowsSales = new List<string>();
            query = "SELECT * FROM stateSales ORDER BY total DESC";
            using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;") )
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        //actual processing of query result
                        while (reader.Read())
                        {
                            string rowName = reader.GetValue(1).ToString();
                            rows.Add(reader.GetValue(0).ToString());
                            dict.Add(reader.GetValue(0).ToString(), new Dictionary<string, string>());
                            rowsSales.Add((rowName == "" ? "0" : rowName));
                        }
                    }
                }
            }   // end of using sqlconnection

            // obtain product sales in top order (50 only)
            List<string> cols = new List<string>();
            List<string> colsSales = new List<string>();
            query2 = "SELECT TOP 50 * FROM productSales ORDER BY total DESC";
            string extra = "SELECT TOP 50 id FROM productSales ORDER BY total DESC";
            using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
            {
                using (SqlCommand cmd = new SqlCommand(query2, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        //actual processing of query result
                        while (reader.Read())
                        {
                            string rowName = reader.GetValue(1).ToString();
                            cols.Add(reader.GetValue(0).ToString());
                            colsSales.Add((rowName == "" ? "0" : rowName));
                        }
                    }
                }
            }   // end of using sqlconnection

            

            // getting sales data for state x product
            query3 = "SELECT state, product_id, SUM(quantity * price) AS total " +
                    "FROM (users INNER JOIN (SELECT * FROM orders WHERE product_id IN (" + extra + "))orders ON users.id = orders.user_id) " +
                    "GROUP BY state, product_id";
            using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=test;Integrated Security=SSPI;"))
            {
                using (SqlCommand cmd = new SqlCommand(query3, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        //actual processing of query result
                        while (reader.Read())
                        {
                            string currEntry = reader.GetValue(0).ToString();
                            if (dict.ContainsKey(currEntry))
                            {
                                dict[currEntry].Add(reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                            }
                        }
                    }
                }
            }   // end of using sqlconnection


            /* note that the constructor need be called without (). I.e., new string[rows.Count, cols.Count]() is wrong */
            string[,] data = new string[rows.Count, cols.Count];
            for (int i = 0; i < rows.Count; i++)
            {

                for (int j = 0; j < cols.Count; j++)
                {
                    // note in C# it is not data[i][j], but rather data[i,j]
                    data[i, j] = dict[rows[i]].ContainsKey(cols[j]) ? dict[rows[i]][cols[j]] : "0";
                }
            }

            Stats s = new Stats();
            
            s.cols = cols;
            s.rows = rows;
            s.data = data;
            s.rowsSales = rowsSales;
            s.colsSales = colsSales;
            return View(s);
        }
    }
}