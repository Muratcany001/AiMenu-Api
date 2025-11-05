using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.DAL.Entitites.AppEntitites
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public  int  MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OrderItemTotalPrice { get; set; }

        public virtual Order Order { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
