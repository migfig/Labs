using Common.Data;
using Common.Data.Models;
using Common.Data.Models.Requests;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;
using System.Web.Http.Controllers;

namespace Common.Controllers
{
    /// <summary>
    /// Categories services
    /// </summary>
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        private readonly IRepository<Category> _repository;

        /// <summary>
        /// Constructor for IOC
        /// </summary>
        /// <param name="repository"></param>
        public CategoriesController(IRepository<Category> repository)
        {
            _repository = repository;
            _repository.Load();            
        }
        
        /// <summary>
        /// Get Categories list
        /// </summary>
        /// <returns>list of categories</returns>
        [Route(""), HttpGet]
        [ResponseType(typeof(IEnumerable<Category>))]
        public IEnumerable<Category> GetCategories()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Get Category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Category</returns>
        [Route("{id:int}"), HttpGet]
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            var category = _repository.GetById(id);
            if (null == category) return BadRequest("Category not found");

            return Ok<Category>(category);
        }

        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="request">Category values</param>
        /// <returns>added Category</returns>
        [Route("add"), HttpPost]
        [ResponseType(typeof(Category))]
        public IHttpActionResult AddCategory([FromBody] CategoryRequest request)
        {
            var category = _repository.Add(new Category
            {
                Id = _repository.GetAll().Max(x => x.Id) + 1,
                Name = request.Name 
            });
            if (null == category) return BadRequest("Invalid values provided");

            return Created<Category>("", category);
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <param name="request">Category values</param>
        /// <returns>updated Category</returns>
        [Route("update/{id:int}"), HttpPut]
        [ResponseType(typeof(Category))]
        public IHttpActionResult UpdateCategory(int id, [FromBody] CategoryRequest request)
        {
            var category = _repository.GetById(id);
            if (null == category) return BadRequest("Category not found or invalid values provided");
            category.Name = request.Name;

            return Ok<Category>(_repository.Update(category));
        }

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>deleted Category</returns>
        [Route("remove/{id:int}"), HttpDelete]
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            var category = _repository.Delete(_repository.GetById(id));
            if (null == category) return BadRequest("Category not found");

            return Ok<Category>(category);
        }
    }
}
