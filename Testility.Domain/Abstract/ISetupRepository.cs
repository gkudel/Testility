using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface ISetupRepository
    {
        IQueryable<TestedClass> TestedClasses { get;  }
    }
}
