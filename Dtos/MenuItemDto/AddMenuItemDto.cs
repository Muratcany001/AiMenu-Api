using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.MenuItemDto
{
    public class AddMenuItemDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Ingeredents { get; set; }
        public string ImageUrl { get; set; }
    }
}
