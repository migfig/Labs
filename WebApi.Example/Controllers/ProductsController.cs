using Common.Data;
using Common.Data.Models;
using Common.Data.Repositories;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;
using Common.Data.Models.Requests;

namespace WebApi.Example.Controllers
{
    /// <summary>
    /// Products services
    /// </summary>
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IRepository<Product> _repository;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsController()
            :this(new ProductRepository())
        {
        }

        /// <summary>
        /// Constructor for IOC
        /// </summary>
        /// <param name="repository"></param>
        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository;
            _repository.Load();
        }

        /// <summary>
        /// Get Products list
        /// </summary>
        /// <returns>list of Products</returns>
        [Route(""), HttpGet]
        [ResponseType(typeof(IEnumerable<Product>))]
        public IEnumerable<Product> GetProducts()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Product</returns>
        [Route("{id:int}"), HttpGet]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            var product = _repository.GetById(id);
            if (null == product) return BadRequest("Product not found");

            return Ok<Product>(product);
        }

        /// <summary>
        /// Add Product
        /// </summary>
        /// <param name="request">Product values</param>
        /// <returns>added Product</returns>
        [Route("add"), HttpPost]
        [ResponseType(typeof(Product))]
        public IHttpActionResult AddProduct([FromBody] ProductRequest request)
        {
            var product = _repository.Add(new Product
            {
                Id = _repository.GetAll().Max(x => x.Id) + 1,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            });
            if (null == product) return BadRequest("Invalid values provided");

            return Created<Product>("", product);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="request">Product values</param>
        /// <returns>updated Product</returns>
        [Route("update/{id:int}"), HttpPut]
        [ResponseType(typeof(Product))]
        public IHttpActionResult UpdateProduct(int id, [FromBody] ProductRequest request)
        {
            var product = _repository.GetById(id);
            if (null == product) return BadRequest("Product not found or invalid values provided");
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;

            return Ok<Product>(_repository.Update(product));
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>deleted Product</returns>
        [Route("delete/{id:int}"), HttpDelete]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            var product = _repository.Delete(_repository.GetById(id));
            if (null == product) return BadRequest("Product not found");

            return Ok<Product>(product);
        }
    }
}
