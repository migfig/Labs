using Common.Data;
using Common.Data.Models;
using Common.Data.Repositories;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApi.Example.Controllers
{
    /// <summary>
    /// Categories services
    /// </summary>
    [RoutePrefix("categories")]
    public class CategoriesController : ApiController
    {
        private readonly IRepository<Category> _repository;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CategoriesController()
            :this(new CategoryRepository())
        {
        }

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
        /// <param name="categoryValues">Category values</param>
        /// <returns>added Category</returns>
        [Route("add"), HttpPost]
        [ResponseType(typeof(Category))]
        public IHttpActionResult AddCategory([FromBody] Category categoryValues)
        {
            var category = _repository.Add(categoryValues);
            if (null == category) return BadRequest("Invalid values provided");

            return Created<Category>("", category);
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <param name="categoryValues">Category values</param>
        /// <returns>updated Category</returns>
        [Route("update/{id:int}"), HttpPut]
        [ResponseType(typeof(Category))]
        public IHttpActionResult UpdateCategory(int id, [FromBody] Category categoryValues)
        {
            var category = _repository.GetById(id);
            if (null == category) return BadRequest("Category not found or invalid values provided");

            return Ok<Category>(_repository.Update(category));
        }

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>deleted Category</returns>
        [Route("delete/{id:int}"), HttpDelete]
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            var category = _repository.Delete(_repository.GetById(id));
            if (null == category) return BadRequest("Category not found");

            return Ok<Category>(category);
        }
    }
}
