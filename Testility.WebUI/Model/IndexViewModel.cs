using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;
using Testility.WebUI.Model;

namespace Testility.WebUI.Model
{
    public class IndexViewModel<T> where T : class
    {
        public IEnumerable<T> List { get; set; }
        public T Selected { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
