using Common.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.Data.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private List<Customer> _customers;

        public Customer Add(Customer item)
        {
            _customers.Add(item);
            Save();
            return item;
        }

        public Customer Delete(Customer item)
        {
            _customers.Remove(item);
            Save();
            return item;
        }

        public IEnumerable<Customer> GetAll()
        {            
            return _customers;
        }

        public Customer GetById(int id)
        {
            return _customers.FirstOrDefault(x => x.Id == id);
        }

        private string XmlFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "customers.xml"); }
        }

        public bool Load()
        {
            if(File.Exists(XmlFile))
            {
                _customers = XmlHelper<Customers>.Load(XmlFile).Items;
            }
            else
            {
                _customers = new List<Customer>();
                _customers.Add(new Customer
                {
                    Id = 1,
                    Name = "ACME Services",
                    Contacts = new List<Contact>
                    {
                        new Contact
                        {
                            Id = 1,
                            FirstName = "Marie",
                            LastName = "Curie",
                            PhoneNumber = "+52 123234345"
                        },
                        new Contact
                        {
                            Id = 2,
                            FirstName = "Joseph",
                            LastName = "Bellevue",
                            PhoneNumber = "+52 321234345"
                        }
                    },
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            Id = 1,
                            RequestedBy = 1,
                            DateCreated = DateTime.Now.Subtract(new TimeSpan(8,0,0,0,0)),
                            DateDelivered = DateTime.Now.Subtract(new TimeSpan(2,0,0,0,0)),
                            Amount = 130.0M,
                            Items = new List<OrderItem>
                            {
                                new OrderItem
                                {
                                    Id = 1,
                                    Product = new Product
                                    {
                                        Id = 1,
                                        Name = "TV",
                                        Description = "LET TV 24 inches",
                                        Price = 114.0M,
                                        Category = new Category
                                        {
                                            Id = 1,
                                            Name = "Electronics"
                                        }
                                    }
                                },
                                new OrderItem
                                {
                                    Id = 2,
                                    Product = new Product
                                    {
                                        Id = 2,
                                        Name = "TV Antenna",
                                        Description = "TV Tuner 23 feet",
                                        Price = 16.0M,
                                        Category = new Category
                                        {
                                            Id = 1,
                                            Name = "Electronics"
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                _customers.Add(new Customer
                {
                    Id = 2,
                    Name = "Retro Supplies",
                    Contacts = new List<Contact>
                    {
                        new Contact
                        {
                            Id = 3,
                            FirstName = "Jovany",
                            LastName = "Marls",
                            PhoneNumber = "+52 324234345"
                        },
                        new Contact
                        {
                            Id = 4,
                            FirstName = "Eve",
                            LastName = "Marling",
                            PhoneNumber = "+52 3654767845"
                        }
                    },
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            Id = 2,
                            RequestedBy = 3,
                            DateCreated = DateTime.Now.Subtract(new TimeSpan(8,0,0,0,0)),
                            DateDelivered = DateTime.Now.Subtract(new TimeSpan(2,0,0,0,0)),
                            Amount = 30.0M,
                            Items = new List<OrderItem>
                            {
                                new OrderItem
                                {
                                    Id = 3,
                                    Product = new Product
                                    {
                                        Id = 3,
                                        Name = "Razor",
                                        Description = "Razor Machine",
                                        Price = 14.0M,
                                        Category = new Category
                                        {
                                            Id = 1,
                                            Name = "Electronics"
                                        }
                                    }
                                },
                                new OrderItem
                                {
                                    Id = 4,
                                    Product = new Product
                                    {
                                        Id = 4,
                                        Name = "Chair",
                                        Description = "Green camping Chair",
                                        Price = 16.0M,
                                        Category = new Category
                                        {
                                            Id = 2,
                                            Name = "Outdoors"
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                Save();
            }

            return _customers.Count > 0;
        }

        public bool Save()
        {
            if (File.Exists(XmlFile))
                File.Delete(XmlFile);

            XmlHelper<Customers>.Save(XmlFile, new Customers { Items = _customers });

            return File.Exists(XmlFile);
        }

        public Customer Update(Customer item)
        {
            var customer = _customers.FirstOrDefault(x => x.Id == item.Id);
            Delete(customer);
            return Add(item);
        }
    }
}
