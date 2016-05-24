using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Shoppa.Models
{
    public class Cart
    {
        public int ID { get; set; }

        public string CartID { get; set; }

        public int ProductID { get; set; }

        public int Count { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}