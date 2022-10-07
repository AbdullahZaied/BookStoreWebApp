using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Layer.Models
{
    public class OrderModelBusiness
    {
        public string UserId { get; set; }
        public int BookId { get; set; }
        public int OrderAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
