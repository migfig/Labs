using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Example.Controllers;
using FluentTesting;
using System.Reflection;

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
            "This test fixture verifies Groups controller works fine"
                .On(_controller)
                .Call("GetGroups")
                .WithParams()
                .ThenVerifyProps(new NameValue("Count()", 2, MemberTypes.Method));
        }

        [TestMethod]
        public void Groups_Controller_GetById_Test()
        {
            "This test fixture verifies Groups controller works fine when getting a group by id"
                .On(_controller)
                .Call("GetGroup")
                .WithParams(Guid.Parse("302114b5-bc91-4def-94b4-d0a069a4dfb6"))
                .ThenVerifyProps(new NameValue("GroupNumber", 2));
        }
    }
}
