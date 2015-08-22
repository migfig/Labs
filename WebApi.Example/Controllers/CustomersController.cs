using Common.Data;
using Common.Data.Models;
using Common.Data.Repositories;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApi.Example.Controllers
{
    /// <summary>
    /// Customer services 
    /// </summary>
    [RoutePrefix("customers")]
    public class CustomersController : ApiController
    {
        private readonly IRepository<Customer> _repository;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomersController()
            :this(new CustomerRepository())
        {
        }

        /// <summary>
        /// Constructor for IOC
        /// </summary>
        /// <param name="repository"></param>
        public CustomersController(IRepository<Customer> repository)
        {
            _repository = repository;
            _repository.Load();
        }

        /// <summary>
        /// Get customers list
        /// </summary>
        /// <returns>list of customers</returns>
        [Route(""), HttpGet]
        [ResponseType(typeof(IEnumerable<Customer>))]
        public IEnumerable<Customer> GetCustomers()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Get customer
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>customer</returns>
        [Route("{id:int}"), HttpGet]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _repository.GetById(id);
            if (null == customer) return BadRequest("Customer not found");

            return Ok<Customer>(customer);
        }

        /// <summary>
        /// Add customer
        /// </summary>
        /// <param name="customerValues">customer values</param>
        /// <returns>added customer</returns>
        [Route("add"), HttpPost]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult AddCustomer([FromBody] Customer customerValues)
        {
            var customer = _repository.Add(customerValues);
            if (null == customer) return BadRequest("Invalid values provided");

            return Created<Customer>("", customer);
        }

        /// <summary>
        /// Update customer
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="customerValues">customer values</param>
        /// <returns>updated customer</returns>
        [Route("update/{id:int}"), HttpPut]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult UpdateCustomer(int id, [FromBody] Customer customerValues)
        {
            var customer = _repository.GetById(id);
            if (null == customer) return BadRequest("Customer not found or invalid values provided");

            return Ok<Customer>(_repository.Update(customer));
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>deleted customer</returns>
        [Route("delete/{id:int}"), HttpDelete]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            var customer = _repository.Delete(_repository.GetById(id));
            if (null == customer) return BadRequest("Customer not found");

            return Ok<Customer>(customer);
        }
    }
}
