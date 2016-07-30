using Common.Data;
using Common.Data.Models;
using Common.Data.Models.Requests;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;
using System;

namespace Common.Controllers
{
    /// <summary>
    /// Customer services 
    /// </summary>
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private readonly IRepository<Customer> _repository;

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
        /// <param name="request">customer values</param>
        /// <returns>added customer</returns>
        [Route("add"), HttpPost]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult AddCustomer([FromBody] CustomerRequest request)
        {
            if (null == request) return BadRequest("Invalid values provided");

            var customer = _repository.Add(new Customer
            {
                Id = _repository.GetAll().Max(x => x.Id) + 1,
                Name = request.Name
            });
            if (null == customer) return BadRequest("Invalid customer");

            return Created<Customer>("", customer);
        }

        /// <summary>
        /// Add customer
        /// </summary>
        /// <param name="request">customer values</param>
        /// <returns>added customer</returns>
        [Route("add/complete"), HttpPost]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult AddCustomerCompleteRequest([FromBody] CustomerCompleteRequest request)
        {
            if (null == request) return BadRequest("Invalid values provided");
            var customers = _repository.GetAll();

            var customer = _repository.Add(new Customer
            {
                Id = customers.Max(x => x.Id) + 1,
                Name = request.Name
            });

            if(request.Contacts != null)
            {
                foreach(var c in request.Contacts)
                {
                    customer.Contacts.Add(new Contact
                    {
                        Id = (from cust in customers
                              select cust.Contacts.Max(x => x.Id)).Max(x => x) + 1,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        PhoneNumber = c.PhoneNumber
                    });
                }
            }

            if(request.Orders != null)
            {
                foreach(var o in request.Orders)
                {
                    customer.Orders.Add(new Order
                    {
                        Id = (from cust in customers
                              select cust.Orders.Max(x => x.Id)).Max(x => x) + 1,
                         RequestedBy = 1,
                         DateCreated = DateTime.Now,
                         DateDelivered = DateTime.Now
                    });

                    var order = customer.Orders.Last();
                    foreach(var item in o.Items)
                    {
                        order.Items.Add(new OrderItem
                        {
                            Id = (from cust in customers
                                  from ord in cust.Orders
                                  select ord.Items.Max(x => x.Id)).Max(x => x) + 1,
                            Product = new Product
                            {
                                Id = (from cust in customers
                                  from ord in cust.Orders
                                  select ord.Items.Max(x => x.Product.Id)).Max(x => x) + 1,
                                 Name = item.Product.Name,
                                 Description = item.Product.Description,
                                 Price = item.Product.Price
                            },
                             Quantity = item.Quantity
                        });
                    }
                }
            }
            
            if (null == customer) return BadRequest("Invalid customer");

            return Created<Customer>("", customer);
        }

        /// <summary>
        /// Update customer
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="request">customer values</param>
        /// <returns>updated customer</returns>
        [Route("update/{id:int}"), HttpPut]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult UpdateCustomer(int id, [FromBody] CustomerRequest request)
        {
            if (null == request) return BadRequest("Invalid values provided");

            var customer = _repository.GetById(id);
            if (null == customer) return BadRequest("Customer not found or invalid values provided");
            customer.Name = request.Name;

            return Ok<Customer>(_repository.Update(customer));
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>deleted customer</returns>
        [Route("remove/{id:int}"), HttpDelete]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            var customer = _repository.Delete(_repository.GetById(id));
            if (null == customer) return BadRequest("Customer not found");

            return Ok<Customer>(customer);
        }
    }
}
