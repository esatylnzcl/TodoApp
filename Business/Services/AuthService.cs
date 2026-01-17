using Core.Dtos.Auth;
using Core.Helpers;
using Core.IRepositories;
using Core.IServices;
using Core.Results;
using Entity.Concrete;
using Microsoft.Extensions.Options;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;


    public AuthService(
        IUserRepository userRepository , 
        ITokenService tokenService )
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public async Task<DataResponse<TokenDto>> Login(LoginDto loginDto)
    {
        var user = await _userRepository.FindByUsername(loginDto.Username);

        if (user == null)
        {
            return DataResponse<TokenDto>.ErrorDateResponse(null , "Wrong password or usernmae");
        }
        
        if (!VerifyPassword(loginDto.Password, user.HashedPassword))
        {
            return DataResponse<TokenDto>.ErrorDateResponse(null ,"Wrong username or password");
        }

        var tokenDto = _tokenService.GenerateAccessToken(user);
        
        return DataResponse<TokenDto>.SuccessDataResponse(tokenDto,"Sucessfully logged in");
    }

    public async Task<Response> Register(RegisterDto registerDto)
    {
        if(await _userRepository.AnyAsync(u => u.Email == registerDto.Email))
        {
            return Response.ErrorResponse("This email has already registered");
        }

        if (await _userRepository.AnyAsync(u => u.Username == registerDto.Username))
        {
            return Response.ErrorResponse("This username is already in use");
        }

        registerDto.Password = HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            HashedPassword = registerDto.Password,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName
        };

        var result = await _userRepository.AddAsync(user);

        if (!result)
        {
            return Response.ErrorResponse("Unknown Error");
        }

        await _userRepository.SaveAsync();

        return Response.SuccessResponse("Successfully registered");
    }
}