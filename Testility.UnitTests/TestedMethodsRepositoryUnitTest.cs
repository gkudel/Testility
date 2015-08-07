using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.Domain.Concrete;
namespace Testility.UnitTests
{
    [TestClass]
    public class TestedMethodsRepositoryUnitTest
    {

        public Mock<DbSet<TestedMethod>> MockSet { get; set; }
        public Mock<EFDbContext> MockContext { get; set; }
        public Mock<TestedMethodsService> ServiceMock { get; set; }


        [TestInitialize]
        public void Int()
        {
            var data = new List<TestedMethod> 
            {  
                new TestedMethod() {Id = 1, Description = "blbla", Name = "ok"}
 
            }.AsQueryable();

            MockSet = new Mock<DbSet<TestedMethod>>();
            MockSet.As<IQueryable<TestedMethod>>().Setup(x => x.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<TestedMethod>>().Setup(x => x.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<TestedMethod>>().Setup(x => x.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<TestedMethod>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator);

            MockContext = new Mock<EFDbContext>();
            MockContext.Setup(x => x.TestedMethods).Returns(MockSet.Object);

            ServiceMock = new Mock<TestedMethodsService>(MockContext.Object);
        }

        [TestMethod]
        public void Test_Get_Methods() 
        {
            Assert.AreEqual(1, ServiceMock.Object.GetMethods().Count);
        }

        [TestMethod]
        public void Test_Add_Methods()
        {
            ServiceMock.Object.SaveTestedMethod(null);
            MockContext.Verify(x => x.SaveChanges(), Times.Never);

        }
    }
}
