using School.Application.Invites.DTOs;

namespace School.Application.Invites.Interfaces;

public interface IInviteAcceptanceService
{
    Task AcceptAsync(AcceptInviteRequest request);
    Task SetPasswordAsync(SetPasswordRequest request);
}
