using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoppa.Models
{
    public class Data
    {
        public List<string> Fields { get; set; }
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        public List<string> Values = new List<string>();
    }
}