using School.Application.Students.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Students.Interfaces
{
    public interface IStudentService
    {
        Task<Guid> UpsertAsync(UpsertStudentRequest request);

        Task<List<StudentListItem>> GetAllAsync();
        Task<StudentDetailsDto> GetByIdAsync(Guid id);
        Task UpdateStatusAsync(Guid id, bool isActive);
    }


}
