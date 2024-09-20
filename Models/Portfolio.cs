using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProgettoWebAPI_Stocks.Models
{
    // Join Table
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; }
        public int StockId { get; set; }

        // Navigation Properties
        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; }
    }
}