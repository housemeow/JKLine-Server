using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JKLineWebServices;
using JKLineWebServices.Controllers;
using JKLineWebServices.Models;

namespace JKLineWebServices.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            MemberController controller = new MemberController();
            JKLineDb jklineDb = new JKLineDb();
            // Act

            // Assert

            Assert.IsTrue(jklineDb.HasInvited(1, 2));
            Assert.IsTrue(jklineDb.HasInvited(1, 3));
            //Assert.IsFalse(jklineDb.HasInvited(2, 1));
        }
    }
}
