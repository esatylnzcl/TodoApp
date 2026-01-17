using Core.Dtos.Auth;
using Core.Results;

namespace Core.IServices;

public interface IAuthService
{
    Task<DataResponse<TokenDto>> Login(LoginDto loginDto);
    Task<Response> Register(RegisterDto registerDto);
}