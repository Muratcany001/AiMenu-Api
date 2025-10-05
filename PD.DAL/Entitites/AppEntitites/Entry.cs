using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.DAL.Entitites.AppEntitites
{
    public class Entry
    {
        public int Id { get; set; }
        public int PlateId { get; set; }
        public DateTime EntryTime { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }


        [ForeignKey("PlateId")]
        public virtual Plate Plate { get; set; }
    }
}
