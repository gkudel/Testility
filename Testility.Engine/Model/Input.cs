﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Model
{
    [Serializable]
    public class Input
    {
        public string SolutionName { get; set; }
        public string Language { get; set; }
        public string[] Code { get; set; }
        public string[] ReferencedAssemblies { get; set; }
    }
}
