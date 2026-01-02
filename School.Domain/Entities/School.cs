using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class School : BaseEntity
{
    public string Name { get; set; }
    public string Board { get; set; }
    public string Medium { get; set; }
    public bool IsActive { get; set; }
}

