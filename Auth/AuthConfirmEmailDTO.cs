namespace flow_view.Auth;

public class AuthConfirmEmailDTO
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
}
