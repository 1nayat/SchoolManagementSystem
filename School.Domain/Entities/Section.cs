using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class Section : SoftDeleteEntity
{
    public string SectionName { get; set; } 
    public Guid ClassId { get; set; }

    public Class Class { get; set; }
}
