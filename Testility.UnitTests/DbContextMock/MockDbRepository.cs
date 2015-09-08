using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

namespace Testility.UnitTests.DbContextMock
{
    public class MockDbRepository 
    {
        #region Members
        private Mock<IDbRepository> _mock;
        public IDbRepository Repository
        {
            get
            {
                if (_mock == null) Init();
                return _mock.Object;
            }
        }
        public Mock<IDbRepository> Mock
        {
            get
            {
                if (_mock == null) Init();
                return _mock;
            }
        }
        #endregion Members

        public MockDbRepository()
        {
        }

        private void Init()
        {
            IQueryable<SetupSolution> SolutionList = new List<SetupSolution>
            {
                new SetupSolution() {Id = 1, Name = "ko", Language= Language.CSharp},
                new SetupSolution() {Id = 2, Name = "ko", Language= Language.CSharp},
                new SetupSolution() {Id = 3, Name = "ko", Language= Language.CSharp},
                new SetupSolution() {Id = 4, Name = "ko", Language= Language.CSharp}

            }.AsQueryable();

            IQueryable<Reference> ReferenceList = new List<Reference>
            {
                new Reference() {Id = 1, Name = "System.dll"}
            }.AsQueryable();

            _mock = new Mock<IDbRepository>();
            _mock.Setup(x => x.GetSetupSolutions(It.IsAny<bool>())).Returns(SolutionList);
            _mock.Setup(x => x.DeleteSetupSolution(It.IsAny<int>())).Returns(true);
            _mock.Setup(x => x.IsAlreadyDefined(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
        }
    }
}
