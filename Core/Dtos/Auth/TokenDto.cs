namespace Core.Dtos.Auth;

public class TokenDto
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}