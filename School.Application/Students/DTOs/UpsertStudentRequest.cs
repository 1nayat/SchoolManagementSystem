using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Students.DTOs
{
    public class UpsertStudentRequest
    {
        public Guid? Id { get; set; } 

        public string AdmissionNumber { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;

        public Guid ClassId { get; set; }
        public Guid SectionId { get; set; }
    }

}
