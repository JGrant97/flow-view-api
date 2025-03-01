namespace flow_view.Auth;

public class AuthRefreshDTO
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}
