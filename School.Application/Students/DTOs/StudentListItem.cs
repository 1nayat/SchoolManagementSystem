using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Students.DTOs
{
    public class StudentListItem
    {
        public Guid Id { get; set; }
        public string AdmissionNumber { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string ClassSection { get; set; } = null!;
        public bool IsActive { get; set; }
    }

}
