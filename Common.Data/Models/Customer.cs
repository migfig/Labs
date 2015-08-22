using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Common.Data.Models
{
    public class Customers
    {
        public virtual List<Customer> Items { get; set; }
    }

    public class Products
    {
        public virtual List<Product> Items { get; set; }
    }

    public class Categories
    {
        public virtual List<Category> Items { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Contact> Contacts { get; set; }
        public virtual List<Order> Orders { get; set; }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int RequestedBy { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateDelivered { get; set; }
        public virtual List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public virtual Category Category { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}