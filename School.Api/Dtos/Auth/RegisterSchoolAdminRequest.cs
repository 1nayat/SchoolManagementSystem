namespace School.Api.Dtos.Auth
{
    public class RegisterSchoolAdminRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}
