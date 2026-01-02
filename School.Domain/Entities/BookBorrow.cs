using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class BookBorrow  : SoftDeleteEntity
{
    public Guid BookId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public Book Book { get; set; }
    public Student Student { get; set; }
}

