namespace Core.IRepositories;
using Entity.Concrete;
using Core.Dtos.Auth;
public interface IUserRepository :  IBaseRepository<User>
{
    Task<User?> FindByUsername(string username);
    Task<List<User>> Filter(UserFilterDto dto);
}