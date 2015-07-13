using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Example.Controllers;
using FluentTesting;
using System.Reflection;
using System.Web.Http.Results;
using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;

namespace WebApi.Example.Tests
{
    [TestClass]
    public class UnitTest
    {
        private GroupsController _controller;

        [TestInitialize]
        public void CreateController()
        {
            _controller = new GroupsController();
        }

        [TestCleanup]
        public void DestroyController()
        {
            _controller = null;
        }

        [TestMethod]
        public void Groups_Controller_GetAll_Test()
        {
            //var groups = _controller.GetGroups();
            //Assert.IsNotNull(groups);
            //Assert.IsTrue(groups.Any());
            "This test fixture verifies Groups controller works fine"
                .On(_controller)
                .Call("GetGroups")
                .WithParams()
                .ThenVerifyProps(
                    new Func<object, object>(o =>
                    {
                        return (o as IQueryable<Models.Group>)
                            .ToList(); 
                    }),
                    new NameValue("Count", 2));
        }

        [TestMethod]
        public void Groups_Controller_GetById_Test()
        {
            //var result = _controller.GetGroup(Guid.Parse("302114b5-bc91-4def-94b4-d0a069a4dfb6")).Result;
            //var group = (result as OkNegotiatedContentResult<Models.Group>).Content;
            //Assert.IsNotNull(group);
            "This test fixture verifies Groups controller works fine when getting a group by id"
                .On(_controller)
                .Call("GetGroup")
                .WithParams(Guid.Parse("302114b5-bc91-4def-94b4-d0a069a4dfb6"))
                .ThenVerifyProps(
                        ResolveResult(),
                        new NameValue("GroupNumber", 2));
        }

        private Func<object, object> ResolveResult()
        {
            return new Func<object, object>(o => 
            {
                return ((o as Task<IHttpActionResult>)
                    .Result as OkNegotiatedContentResult<Models.Group>)
                    .Content as Models.Group;
            });
        }
    }
}
