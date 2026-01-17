using Core.Dtos.Auth;
using Entity.Concrete;

namespace Core.IServices;

public interface ITokenService
{
    TokenDto GenerateAccessToken(User user);
}