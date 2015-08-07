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
    public class SetupRepositoryUnitTest
    {

        public Mock<DbSet<TestedClass>> MockSet { get; set; }
        public Mock<EFDbContext> MockContext { get; set; }
        public Mock<TestedClassesService> ServiceMock { get; set; }


        [TestInitialize]
        public void Int()
        {
            var data = new List<TestedClass> 
            { 
                new TestedClass() {Id = 1,Description = "test",  Name = "ok"},
 
            }.AsQueryable();

            MockSet = new Mock<DbSet<TestedClass>>();
            MockSet.As<IQueryable<TestedClass>>().Setup(x => x.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<TestedClass>>().Setup(x => x.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<TestedClass>>().Setup(x => x.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<TestedClass>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator);

            MockContext = new Mock<EFDbContext>();
            MockContext.Setup(x => x.TestedClasses).Returns(MockSet.Object);

            ServiceMock = new Mock<TestedClassesService>(MockContext.Object);
        }



        [TestMethod]
        public void Test_Get_Testet_Classes()
        {
           Assert.AreEqual(1, ServiceMock.Object.GetTestetClasses().Count());
        }

         [TestMethod]
        public void Test_Delete_Test_Classes()
        {
           ServiceMock.Object.DeleteTestClasses(1);
           MockSet.Verify(x => x.Remove(It.IsAny<TestedClass>()), Times.Once);
           MockContext.Verify(x => x.SaveChanges(), Times.Once);

        }

         [TestMethod]
         public void Test_Add_Test_Classes_If_Object_Null()
         {
             TestedClass testedClass = null;
             ServiceMock.Object.AddTestClasses(testedClass);

             MockSet.Verify(x => x.Add(It.IsAny<TestedClass>()), Times.Never);
             MockContext.Verify(x => x.SaveChanges(), Times.Never);

         }

         [TestMethod]
         public void Test_Add_Test_Classes_If_Object_Not_Null()
         {
             TestedClass testedClass = new TestedClass();
             ServiceMock.Object.AddTestClasses(testedClass);

             MockSet.Verify(x => x.Add(It.IsAny<TestedClass>()), Times.Once);
             MockContext.Verify(x => x.SaveChanges(), Times.Once);

         }

         [TestMethod]
         public void Test_Update_Test_Classes_If_Object_Null()
         {
             TestedClass testedClass = new TestedClass();
             MockContext.Verify(x => x.SaveChanges(), Times.Never);
         }

         [TestMethod]
         public void Test_Update_Test_Classes_If_Object_Not_Null()
         {
             TestedClass testedClass = new TestedClass() { Id = 1, Description = "test", Name = "ok" };
             ServiceMock.Object.UpdateTestClasses(testedClass);
             var result = ServiceMock.Object.DetailsTestedClass(1);
             Assert.AreEqual(1, result.Id);
             MockContext.Verify(x => x.SaveChanges(), Times.Once);

         }

         [TestMethod]
         public void Details_Test_Classes_If_Object_Not_Null()
         {
            TestedClass testedClass =  ServiceMock.Object.DetailsTestedClass(1);
            Assert.AreEqual(1, testedClass.Id);
            
         }

         [TestMethod]
         [ExpectedException(typeof(NullReferenceException))]
         public void Details_Test_Classes_If_Object_Null()
         {
             TestedClass testedClass = ServiceMock.Object.DetailsTestedClass(22);
             Assert.AreEqual(1, testedClass.Id);
          
         }
    }
}
