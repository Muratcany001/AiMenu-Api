using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.DAL.Entitites.AppEntitites
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public int TotalPrice { get; set; }
        public string TableNumber { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new  List<OrderItem>();
    }
}
