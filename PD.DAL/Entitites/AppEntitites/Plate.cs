using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.DAL.Entitites.AppEntitites
{
    public class Plate
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlateNumber { get; set; }
        public bool isResident { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Entry> Entries { get; set; } = new List<Entry>();
        public User User { get; set; }

    }
}
