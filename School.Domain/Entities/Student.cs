using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities
{
    public class Student : SoftDeleteEntity
    {
        public string AdmissionNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }

        public ICollection<StudentGuardian> Guardians { get; set; }
    }
}
