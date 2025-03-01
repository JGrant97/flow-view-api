namespace flow_view.Auth;

public class AuthRegisterDTO
{
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string emailCallbackUrl { get; set; }
}
