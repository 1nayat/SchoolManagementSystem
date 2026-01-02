using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class Class : SoftDeleteEntity
{
    public string ClassName { get; set; } 
}

