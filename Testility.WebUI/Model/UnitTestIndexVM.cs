using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Testility.WebUI.Model
{
    public class UnitTestIndexVM
    {
        public IEnumerable<UnitTestIndexItemVM> List { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public BrowserConfig BrowserConfig { get; set; }
    }
}