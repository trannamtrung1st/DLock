using DLock.Identity.WebApi.Entities;
using DLock.Identity.WebApi.Persistence;
using DLock.Models;
using Microsoft.EntityFrameworkCore;

namespace DLock.Identity.WebApi.Services
{
    public interface IUserService
    {
        Task<UserModel> FindUser(string userName);
        Task CreateUser(string userName);
        Task RemoveUser(string userName);
        Task<IEnumerable<UserModel>> GetAllUsers();
    }

    public class UserService : IUserService
    {
        private readonly IdentityContext _dbContext;

        public UserService(IdentityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateUser(string userName)
        {
            await _dbContext.User.AddAsync(new UserEntity { UserName = userName });

            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserModel> FindUser(string userName)
        {
            var entity = await _dbContext.User.FindAsync(userName);

            if (entity == null) return null;

            return new UserModel
            {
                UserName = entity.UserName,
            };
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var models = await _dbContext.User.Select(u => new UserModel
            {
                UserName = u.UserName,
            }).ToArrayAsync();

            return models;
        }

        public async Task RemoveUser(string userName)
        {
            var entity = await _dbContext.User.FindAsync(userName);

            _dbContext.User.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
