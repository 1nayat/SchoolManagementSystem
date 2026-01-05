using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class Book : SoftDeleteEntity
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int Quantity { get; set; }
}

