using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Models.Requests
{
    public class CustomerRequest
    {
        public string Name { get; set; }
    }

    public class ProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class CategoryRequest
    {
        public string Name { get; set; }
    }
}
