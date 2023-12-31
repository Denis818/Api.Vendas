namespace Domain.Dtos.User
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserInfoDto
    {
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
