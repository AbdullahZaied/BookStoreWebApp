using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Layer.Data
{
    public class Stock
    {
        public int Id { get; set; }
        public int StockAmount { get; set; }
        public DateTime StockedAt { get; set; }
        public string StockedBy { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
