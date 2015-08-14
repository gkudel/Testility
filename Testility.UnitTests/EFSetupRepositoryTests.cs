
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Testility.Domain.Concrete;
using Testility.Domain.Entities;
using Testility.Domain.Mappings;

namespace Testility.UnitTests
{
    [TestClass]
    public class EFSetupRepositoryTests
    {

        public Mock<DbSet<Item>> MockSet { get; set; }
        public Mock<DbSet<TestedClass>> MockTested { get; set; }
        public Mock<DbSet<TestedMethod>> MockMethod { get; set; }
        public Mock<DbSet<Test>> MockTest { get; set; }

        public Mock<EFDbContext> MockContext { get; set; }
        public EFSetupRepository Service { get; set; }

        [TestInitialize]
        public void Init()
        {
            var data = new List<Item> 
            { 
                new Item() {Id = 1, Name = "ok"},
                new Item() {Id = 2, Name = "12ok"}

            }.AsQueryable();

            MockSet = new Mock<DbSet<Item>>();
            MockSet.As<IQueryable<Item>>().Setup(x => x.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<Item>>().Setup(x => x.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<Item>>().Setup(x => x.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<Item>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator);


            var testedData = new List<TestedClass> { }.AsQueryable();

            MockTested = new Mock<DbSet<TestedClass>>();
            MockTested.As<IQueryable<TestedClass>>().Setup(x => x.Provider).Returns(testedData.Provider);
            MockTested.As<IQueryable<TestedClass>>().Setup(x => x.Expression).Returns(testedData.Expression);
            MockTested.As<IQueryable<TestedClass>>().Setup(x => x.ElementType).Returns(testedData.ElementType);
            MockTested.As<IQueryable<TestedClass>>().Setup(x => x.GetEnumerator()).Returns(testedData.GetEnumerator);

            var methodData = new List<TestedMethod> { }.AsQueryable();

            MockMethod = new Mock<DbSet<TestedMethod>>();
            MockMethod.As<IQueryable<TestedMethod>>().Setup(x => x.Provider).Returns(methodData.Provider);
            MockMethod.As<IQueryable<TestedMethod>>().Setup(x => x.Expression).Returns(methodData.Expression);
            MockMethod.As<IQueryable<TestedMethod>>().Setup(x => x.ElementType).Returns(methodData.ElementType);
            MockMethod.As<IQueryable<TestedMethod>>().Setup(x => x.GetEnumerator()).Returns(methodData.GetEnumerator);


            var testData = new List<Test> { }.AsQueryable();

            MockTest = new Mock<DbSet<Test>>();
            MockTest.As<IQueryable<Test>>().Setup(x => x.Provider).Returns(testData.Provider);
            MockTest.As<IQueryable<Test>>().Setup(x => x.Expression).Returns(testData.Expression);
            MockTest.As<IQueryable<Test>>().Setup(x => x.ElementType).Returns(testData.ElementType);
            MockTest.As<IQueryable<Test>>().Setup(x => x.GetEnumerator()).Returns(testData.GetEnumerator);

            MockContext = new Mock<EFDbContext>();

            MockContext.Setup(x => x.SourCodes).Returns(MockSet.Object);
            MockContext.Setup(x => x.TestedClasses).Returns(MockTested.Object);
            MockContext.Setup(x => x.TestedMethods).Returns(MockMethod.Object);
            MockContext.Setup(x => x.Tests).Returns(MockTest.Object);

            Service = new EFSetupRepository(MockContext.Object);
        }

        [TestMethod]
        public void Can_Get_All_Source_Codes()
        {
            Assert.AreEqual(2, Service.GetAllSourceCodes().Count());
        }

        [TestMethod]
        public void Cannot_Delete_Source_Code_With_WrongId()
        {
            var result = Service.Delete(10);
            MockSet.Verify(m => m.Remove(It.IsAny<Item>()), Times.Never);
            MockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Can_Delete_Source_Code()
        {
            Service.Delete(1);
            MockSet.Verify(m => m.Remove(It.IsAny<Item>()), Times.Once);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Cannot_GetSourceCode_NullReturn()
        {
            var result = Service.GetSourceCode(null);
            Assert.AreEqual(null,result);
        }
    }
}
