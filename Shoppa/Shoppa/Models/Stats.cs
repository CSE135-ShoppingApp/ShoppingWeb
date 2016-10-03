using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
    
namespace Shoppa.Models
{
    public class Stats
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Shoppa.Models.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public List<string> rows { get; set; }
        public List<string> cols { get; set; }
        public List<string> rowsSales { get; set; }
        public List<string> colsSales { get; set; }
        public string[,] data { get; set; }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}