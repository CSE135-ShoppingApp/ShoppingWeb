//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shoppa.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class order
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public bool is_cart { get; set; }
    
        public virtual product product { get; set; }
        public virtual user user { get; set; }
    }
}
