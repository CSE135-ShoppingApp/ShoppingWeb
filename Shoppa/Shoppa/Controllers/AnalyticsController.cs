using Shoppa.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppa.Controllers
{
    [AllowAnonymous]
    public class AnalyticsController : Controller
    {
        shoppa135dbEntities context = new shoppa135dbEntities();

        // GET: Analytics
        public ActionResult Sales(string rowsMenu, string orderMenu, string productCategories, int rowPage = 0, int columnPage = 0)
        {
            // FILTERING OPTIONS:
            string[] rowsMenuList = new string[] { "Customers", "States" };
            // Select default and assign list
            ViewBag.RowsMenu = new SelectList(rowsMenuList, rowsMenu ?? "Customers");
            string[] orderMenuList = new string[] { "Alphabetical", "Top-K" };
            ViewBag.OrderMenu = new SelectList(orderMenuList, orderMenu ?? "Alphabetical");
            var productCategoriesList = context.categories.Select(x => x.name).ToList();
            ViewBag.ProductCategories = new SelectList(productCategoriesList, productCategories ?? "All");

            string rows = "name";
            if (rowsMenu == "States")
            {
                rows = "state";
            }

            string order = "alphabet";
            if (orderMenu == "Top-K")
            {
                order = "topk";
            }

            int startRow = (rowPage * 20) + 1;
            int startColumn = (columnPage * 10) + 1;

            ViewBag.RowPage = rowPage;
            ViewBag.ColumnPage = columnPage;

            Data d = new Data();

            using (SqlConnection con = new SqlConnection("Server=tcp:shoppa135dbserver.database.windows.net,1433;Database=shoppa135db;User ID=shoppa135dbuser@shoppa135dbserver;Password=@C5e1352016;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@rows", SqlDbType.VarChar).Value = rows;
                    cmd.Parameters.Add("@order", SqlDbType.VarChar).Value = order;
                    cmd.Parameters.Add("@startRow", SqlDbType.Int).Value = startRow;
                    cmd.Parameters.Add("@startColumn", SqlDbType.Int).Value = startColumn;

                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<string> fields = new List<string>();
                        List<Row> rowsL = new List<Row>();

                        while (reader.Read())
                        {
                            if (fields.Count() == 0)
                            {

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    fields.Add(reader.GetName(i));
                                }
                            }

                            Row row = new Row();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Values.Add(reader.GetValue(i).ToString());
                            }
                            rowsL.Add(row);
                        }


                        d.Fields = fields;
                        d.Rows = rowsL;
                    }
                }
            }
            
            return View(d);
        }

        private string query = @"DECLARE @cols            AS NVARCHAR(MAX),
        @colsWithNoNulls AS NVARCHAR(MAX),
        @query           AS NVARCHAR(MAX),
		@colsQueryHelper AS NVARCHAR(MAX),
		@rowsQueryHelper AS NVARCHAR(MAX)

SET @rowsQueryHelper =
	(CASE WHEN @order = 'topk'
		THEN
			'SELECT rows, rank FROM (SELECT users.'+@rows+' as rows, RANK() OVER (ORDER BY SUM(orders.price*orders.quantity) DESC) AS rank
				FROM users LEFT JOIN orders ON orders.user_id = users.id
				GROUP BY users.'+@rows + ') ranks
				WHERE rank BETWEEN ' + CAST(@startRow AS VARCHAR(MAX)) +' AND '+ CAST((@startRow+19) AS VARCHAR(MAX))

		ELSE
			'SELECT rows, rank FROM (SELECT users.'+@rows+' as rows, RANK() OVER (ORDER BY users.'+@rows+') AS rank FROM users) ranks
				WHERE rank BETWEEN ' + CAST(@startRow AS VARCHAR(MAX)) +' AND '+ CAST((@startRow+19) AS VARCHAR(MAX))
	END)

SET @colsQueryHelper =
	(CASE WHEN @order = 'topk'
		THEN
			'SELECT name FROM (
			SELECT products.name AS name, RANK() OVER (ORDER BY SUM(orders.price*orders.quantity) DESC) AS rank
			FROM products LEFT JOIN orders ON orders.product_id = products.id
			GROUP BY products.name) products
			WHERE rank BETWEEN ' + CAST(@startColumn AS VARCHAR(MAX)) +' AND '+ CAST((@startColumn+9) AS VARCHAR(MAX))
		ELSE
			'SELECT name FROM (
			SELECT products.name AS name, RANK() OVER (ORDER BY name) AS rank
			FROM products) products
			WHERE rank BETWEEN ' + CAST(@startColumn AS VARCHAR(MAX)) +' AND '+ CAST((@startColumn+9) AS VARCHAR(MAX))
	END)

CREATE TABLE #TEMP (name VARCHAR(MAX))
INSERT INTO #TEMP 
EXECUTE sp_executesql @colsQueryHelper

SET @cols = STUFF((SELECT ',' + QUOTENAME(name) FROM (SELECT name from #TEMP) products FOR XML PATH (''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '' ) 

SET @colsWithNoNulls = STUFF((SELECT ', ISNULL(' + QUOTENAME(name) + ', 0) ' + QUOTENAME(name) FROM (
			SELECT name FROM #TEMP) products FOR XML PATH (''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')

SET @query = 'SELECT col, ' + @colsWithNoNulls + ' FROM 
             (
                 SELECT users.'+@rows+' as col, rank, products.name as name, (orders.price*orders.quantity) as price
				 FROM users
				 JOIN 
				 (
					'+@rowsQueryHelper+'
				 ) ranks ON users.'+@rows+' = ranks.rows
				 LEFT JOIN orders  ON orders.user_id = users.id
				 LEFT JOIN products ON orders.product_id = products.id

             ) x
             PIVOT 
             (
                 SUM(price)
                 FOR name IN (' + @cols + ')
             ) p'

EXECUTE (@query)
DROP TABLE #TEMP";
    }
}