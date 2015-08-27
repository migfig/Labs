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

    public class CustomerCompleteRequest
    {
        public string Name { get; set; }
        public virtual List<ContactRequest> Contacts { get; set; }
        public virtual List<OrderRequest> Orders { get; set; }
    }

    public class ContactRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class OrderRequest
    {
        public int RequestedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateDelivered { get; set; }
        public virtual List<OrderItemRequest> Items { get; set; }
    }

    public class OrderItemRequest
    {
        public virtual ProductRequest Product { get; set; }
        public int Quantity { get; set; }
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
