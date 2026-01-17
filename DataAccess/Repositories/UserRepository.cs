using Core.Dtos.Auth;
using Core.IRepositories;
using DataAccess.Context;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

/*
 * 2 tane bağımsız
 * (Yani tek bi apide bu 2 entity'i aynı anda etkileyecek bir durum api,fonksiyon olmadığından dolayı bağımsız diyorum.)
 * bu yüzden ayrı ayrı repository açıp, rollback , commit gibi fonkların direk implemente edildiği unitofwork yapısı kullanmadım.
 * bence overengineering olurdu
 */


public class UserRepository : BaseRepository<User> , IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> FindByUsername(string username)
    {

        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
        return user;
    }

    public async Task<List<User>> Filter(UserFilterDto dto)
    {
        IQueryable<User> query = _context.Users;

        if (!string.IsNullOrEmpty(dto.Username))
            query = query.Where(x => x.Username.Contains(dto.Username));

        if (!string.IsNullOrEmpty(dto.Email))
            query = query.Where(x => x.Email.Contains(dto.Email));

        if (!string.IsNullOrEmpty(dto.FirstName))
            query = query.Where(x => x.FirstName.Contains(dto.FirstName));

        if (!string.IsNullOrEmpty(dto.LastName))
            query = query.Where(x => x.LastName.Contains(dto.LastName));
        
        if (dto.IsDeleted.HasValue)
            query = query.Where(x => x.IsDeleted == dto.IsDeleted.Value);

        return await query.ToListAsync();
    }
}