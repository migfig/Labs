using Common.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.Data.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private List<Category> _categories;

        public Category Add(Category item)
        {
            _categories.Add(item);
            Save();
            return item;
        }

        public Category Delete(Category item)
        {
            _categories.Remove(item);
            Save();
            return item;
        }

        public IEnumerable<Category> GetAll()
        {            
            return _categories;
        }

        public Category GetById(int id)
        {
            return _categories.FirstOrDefault(x => x.Id == id);
        }

        private string XmlFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "categories.xml"); }
        }

        public bool Load()
        {
            Common.Extensions.TraceLog.Information("Xml repository: {XmlFile}", XmlFile);
            if(File.Exists(XmlFile))
            {
                _categories = XmlHelper<Categories>.Load(XmlFile).Items;
            }
            else
            {
                _categories = new List<Category>();
                _categories.Add(new Category
                {
                        Id = 1,
                        Name = "Electronics"
                });
                _categories.Add(new Category
                {
                        Id = 2,
                        Name = "Outdoors"
                });

                Save();
            }

            return _categories.Count > 0;
        }

        public bool Save()
        {
            if (File.Exists(XmlFile))
                File.Delete(XmlFile);

            XmlHelper<Categories>.Save(XmlFile, new Categories { Items = _categories });

            return File.Exists(XmlFile);
        }

        public Category Update(Category item)
        {
            var Category = _categories.FirstOrDefault(x => x.Id == item.Id);
            Delete(Category);
            return Add(item);
        }
    }
}
