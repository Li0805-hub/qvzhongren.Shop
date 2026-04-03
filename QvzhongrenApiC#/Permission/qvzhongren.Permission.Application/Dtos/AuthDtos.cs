namespace qvzhongren.Permission.Application.Dtos;

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string? RealName { get; set; }
}
