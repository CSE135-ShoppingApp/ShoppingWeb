using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Shoppa.Models
{
    public class Category
    {
        public int ID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        [Index("IX_UniqueCategoryName", 1, IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [StringLength(160)]
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}