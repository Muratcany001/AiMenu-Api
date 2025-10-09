using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.PlateDto
{
    public class PlateDto
    {
        public int UserId { get; set; }
        public string PlateNumber { get; set; }
        public bool isResident { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
