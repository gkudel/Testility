using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.UnitTests.DbContextMock
{
    public class MockedDbContext<T> : Mock<T> where T : DbContext
    {
        public Dictionary<string, object> Tables
        {
            get { return _Tables ?? (_Tables = new Dictionary<string, object>()); }
        }
        private Dictionary<string, object> _Tables;
    }
}
