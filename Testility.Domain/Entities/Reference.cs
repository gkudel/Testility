﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class Reference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SolutionApi> Solutions { get; set; }
    }
}
