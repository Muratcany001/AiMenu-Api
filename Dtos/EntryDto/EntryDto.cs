using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.EntryDto
{
    public class EntryDto
    {
        public int PlateId { get; set; }
        public DateTime EntryTime { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }

    }
}
