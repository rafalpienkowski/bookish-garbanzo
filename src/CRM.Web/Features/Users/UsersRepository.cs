using System;
using System.Threading.Tasks;
using CRM.Domain.Users;
using CRM.Web.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.Web.Features.Users
{
    public class UsersRepository
    {
        private readonly CrmContext _context;

        public UsersRepository(CrmContext context)
        {
            _context = context;
        }

        public Task<User> GetUserById(Guid userId) =>
            _context.Users.FirstAsync(u => u.Id == userId);

        public async Task Save(User user)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (entity == null)
            {
                return;
            }

            _context.Entry(entity).CurrentValues.SetValues(user);
        }

        public async Task Create(Guid id, string email, UserType type)
        {
            var user = User.CreateNew(id, email, type);
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}