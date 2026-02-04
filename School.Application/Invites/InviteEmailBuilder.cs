using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Invites
{
    public static class InviteEmailBuilder
    {
        public static (string Subject, string Body) BuildTeacherInvite(
            string schoolName,
            string inviteLink
        )
        {
            var subject = $"You’re invited to join {schoolName}";

            var body = $@"
            <p>Hello,</p>

            <p>You have been invited to join <strong>{schoolName}</strong> as a teacher.</p>

            <p>Click the link below to set your password and complete your profile:</p>

            <p>
                <a href='{inviteLink}'>Accept Invitation</a>
            </p>

            <p>This link will expire soon and can only be used once.</p>

            <p>If you did not expect this email, you can safely ignore it.</p>

            <p>— {schoolName} Team</p>
        ";

            return (subject, body);
        }
    }

}
