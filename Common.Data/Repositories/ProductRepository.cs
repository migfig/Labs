using Common.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.Data.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private List<Product> _products;

        public Product Add(Product item)
        {
            _products.Add(item);
            Save();
            return item;
        }

        public Product Delete(Product item)
        {
            _products.Remove(item);
            Save();
            return item;
        }

        public IEnumerable<Product> GetAll()
        {            
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }

        private string XmlFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.xml"); }
        }

        public bool Load()
        {
            if(File.Exists(XmlFile))
            {
                _products = XmlHelper<Products>.Load(XmlFile).Items;
            }
            else
            {
                _products = new List<Product>();
                _products.Add(new Product
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
                });
                _products.Add(new Product
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
                });
                _products.Add(new Product
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
                });
                _products.Add(new Product
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
                });

                Save();
            }

            return _products.Count > 0;
        }

        public bool Save()
        {
            if (File.Exists(XmlFile))
                File.Delete(XmlFile);

            XmlHelper<Products>.Save(XmlFile, new Products { Items = _products });

            return File.Exists(XmlFile);
        }

        public Product Update(Product item)
        {
            var Product = _products.FirstOrDefault(x => x.Id == item.Id);
            Delete(Product);
            return Add(item);
        }
    }
}
