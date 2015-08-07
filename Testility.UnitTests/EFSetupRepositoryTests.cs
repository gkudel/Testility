using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.Domain.Abstract;
using Testility.Domain.Concrete;
using Testility.Domain.Entities;
using Testility.Domain.Mappings;

namespace Testility.UnitTests
{
    [TestClass]
    public class EFSetupRepositoryTests
    {

        public Mock<DbSet<SourceCode>> MockSet { get; set; }
        public Mock<EFDbContext> MockContext { get; set; }
        public Mock<EFSetupRepository> MockService { get; set; }

        [TestInitialize]
        public void Init()
        {
            var data = new List<SourceCode> 
            { 
                new SourceCode() {Id = 1, Name = "ok"},
                new SourceCode() {Id = 2, Name = "12ok"}

            }.AsQueryable();

            MockSet = new Mock<DbSet<SourceCode>>();
            MockSet.As<IQueryable<SourceCode>>().Setup(x => x.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<SourceCode>>().Setup(x => x.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<SourceCode>>().Setup(x => x.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<SourceCode>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator);

            MockContext = new Mock<EFDbContext>();
            MockContext.Setup(x => x.SourCodes).Returns(MockSet.Object);
            //MockContext.Setup(x=>x.Entry))
            MockService = new Mock<EFSetupRepository>(MockContext.Object);
        }

        [TestMethod]
        public void Can_Get_All_Source_Codes()
        {
            Assert.AreEqual(2, MockService.Object.SourceCodes.Count());
        }

        [TestMethod]
        public void Cannot_Delete_Source_Code_With_WrongId()
        {
            var result = MockService.Object.DeleteSourceCode(10);
            MockSet.Verify(m => m.Remove(It.IsAny<SourceCode>()), Times.Never);
            MockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Can_Delete_Source_Code()
        {
            MockService.Object.DeleteSourceCode(1);
            MockSet.Verify(m => m.Remove(It.IsAny<SourceCode>()), Times.Once);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Cannot_Save_Source_Code_With_ObjectNull()
        {
            MockService.Object.SaveSourceCode(null);
            MockSet.Verify(m => m.Add(It.IsAny<SourceCode>()), Times.Never);
            MockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void Can_Save_Source_Code()
        {
            SourceCode sourceCode = new SourceCode() { Id = 3 };
            MockService.Object.SaveSourceCode(sourceCode);
            MockSet.Verify(m => m.Add(It.IsAny<SourceCode>()), Times.Once);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
        }


        [TestMethod]
        public void Can_Update_Source_Code()
        {
            SourceCode sourceCode = new SourceCode() { Id = 2, Name = "be" };
            AutoMapperConfiguration.Configure();
            MockService.Object.SaveSourceCode(sourceCode);

            SourceCode c = MockSet.Object.FirstOrDefault(s => s.Id == sourceCode.Id);
            Assert.AreEqual(sourceCode.Name, c.Name);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

       

       
    }
}
