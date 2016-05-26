using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Shoppa.Models
{
    public class Data
    {
        #region Query
        private static string query = @"DECLARE @cols            AS NVARCHAR(MAX),
        @colsWithNoNulls AS NVARCHAR(MAX),
        @query           AS NVARCHAR(MAX),
		@colsQueryHelper AS NVARCHAR(MAX),
		@rowsQueryHelper AS NVARCHAR(MAX),
		@categoryQuery AS NVARCHAR(MAX),
		@categoryQueryWHere AS NVARCHAR(MAX)

SET @categoryQueryWhere = CASE WHEN @category_id = 0 THEN '' ELSE ' WHERE products.category_id = '+ CAST(@category_id AS VARCHAR(MAX)) END
SET @categoryQuery = CASE WHEN @category_id = 0 THEN '' ELSE 'LEFT JOIN ( SELECT * FROM products '+@categoryQueryWhere+') products ON orders.product_id = products.id' END

SET @rowsQueryHelper =
	(CASE WHEN @order = 'topk'
		THEN
			'SELECT rows, rank, sum  FROM (SELECT users.'+@rows+' as rows, SUM(orders.price*orders.quantity) AS sum, RANK() OVER (ORDER BY SUM(orders.price*orders.quantity) DESC) AS rank
				FROM users LEFT JOIN orders ON orders.user_id = users.id '+@categoryQuery+'
				GROUP BY users.'+@rows + ') ranks
					WHERE rank BETWEEN ' + CAST(@startRow AS VARCHAR(MAX)) +' AND '+ CAST((@startRow+19) AS VARCHAR(MAX))
				

		ELSE
			'SELECT rows, rank, sum FROM (SELECT users.'+@rows+' as rows, SUM(orders.price*orders.quantity) AS sum, RANK() OVER (ORDER BY users.'+@rows+') AS rank FROM users
			LEFT JOIN orders ON orders.user_id = users.id '+@categoryQuery+'
				GROUP BY users.'+@rows + '
			
			) ranks
				WHERE rank BETWEEN ' + CAST(@startRow AS VARCHAR(MAX)) +' AND '+ CAST((@startRow+19) AS VARCHAR(MAX))
				
	END)

SET @colsQueryHelper =
	(CASE WHEN @order = 'topk'
		THEN
			'SELECT CONCAT(name, '' ('', ISNULL(sum,0), '')''), ISNULL(sum,0), products.id FROM (
			SELECT products.name AS name, SUM(orders.price*orders.quantity) AS sum, products.id, RANK() OVER (ORDER BY SUM(orders.price*orders.quantity) DESC) AS rank
			FROM  (SELECT * FROM products '+@categoryQueryWhere+') products LEFT JOIN orders ON orders.product_id = products.id   
			GROUP BY products.name, products.id) products WHERE rank BETWEEN ' + CAST(@startColumn AS VARCHAR(MAX)) +' AND '+ CAST((@startColumn+9) AS VARCHAR(MAX))
		ELSE
			'SELECT  CONCAT(name, '' ('', ISNULL(sum,0), '')''), ISNULL(sum,0), products.id FROM (
			SELECT products.name AS name, SUM(orders.price*orders.quantity) AS sum, products.id, RANK() OVER (ORDER BY name) AS rank
			FROM (SELECT * FROM products '+@categoryQueryWhere+')  products LEFT JOIN orders ON orders.product_id = products.id GROUP BY products.name, products.id) products  WHERE rank BETWEEN ' + CAST(@startColumn AS VARCHAR(MAX)) +' AND '+ CAST((@startColumn+9) AS VARCHAR(MAX))
	END)

CREATE TABLE #TEMP (name VARCHAR(MAX), price INT, id INT)
INSERT INTO #TEMP 
EXECUTE sp_executesql @colsQueryHelper

SET @cols = STUFF((SELECT ',' + QUOTENAME(name) FROM (SELECT name from #TEMP) products FOR XML PATH ('')), 1, 1, '' )

SET @colsWithNoNulls = STUFF((SELECT ', ISNULL(' + QUOTENAME(name) + ', 0) ' + QUOTENAME(name) FROM (
			SELECT name FROM #TEMP) products FOR XML PATH ('')), 1, 1, '')

SET @query = 'SELECT col, ' + @colsWithNoNulls + ' FROM 
             (
                 SELECT CONCAT(users.'+@rows+', '' ('', ISNULL(ranks.sum,0), '')'') AS col, rank, #TEMP.name AS name, (orders.price*orders.quantity) as price
				 FROM users
				 JOIN 
				 (
					'+@rowsQueryHelper+'
				 ) ranks ON users.'+@rows+' = ranks.rows
				 LEFT JOIN orders  ON orders.user_id = users.id
				 LEFT JOIN #TEMP ON orders.product_id = #TEMP.id
             ) x
             PIVOT 
             (
                 SUM(price)
                 FOR name IN (' + @cols + ')
             ) p'

EXECUTE (@query)
DROP TABLE #TEMP";
        #endregion

        public void PerformQuery(string rowsMenu, string orderMenu, int startRow, int startColumn, int productCategory)
        {
            using (SqlConnection con = new SqlConnection("Server=tcp:shoppa135dbserver.database.windows.net,1433;Database=shoppa135db;User ID=shoppa135dbuser@shoppa135dbserver;Password=@C5e1352016;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"))
            {
                using (SqlCommand cmd = new SqlCommand(Data.query, con))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@rows", SqlDbType.VarChar).Value = rowsMenu;
                    cmd.Parameters.Add("@order", SqlDbType.VarChar).Value = orderMenu;
                    cmd.Parameters.Add("@startRow", SqlDbType.Int).Value = startRow;
                    cmd.Parameters.Add("@startColumn", SqlDbType.Int).Value = startColumn;
                    cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = productCategory;

                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        this.Fields = new List<string>();
                        this.Rows = new List<Row>();

                        while (reader.Read())
                        {
                            if (Fields.Count() == 0)
                            {

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Fields.Add(reader.GetName(i));
                                }
                            }

                            Row row = new Row();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Values.Add(reader.GetValue(i).ToString());
                            }
                            Rows.Add(row);
                        }
                    }
                }
            }
        } 

        public List<string> Fields { get; set; }
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        public List<string> Values = new List<string>();
    }
}