using School.Application.Teachers.DTOs;

namespace School.Application.Teachers.Interfaces;

public interface ITeacherInviteService
{
    Task <string?>InviteAsync(InviteTeacherRequest request);
}
