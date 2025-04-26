using SothbeysKillerApi.Contexts;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository
{
    public class EfUserRepository : IUserRepository
    {
        private readonly AuctionDbContext _context;
        public EfUserRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public User? GetById(Guid id)
        {
            return _context.Users.Find(id);
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User Create(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public User Update(User entity)
        {
            var user = _context.Users.Find(entity.Id);
            if (user == null) return null;
            user.Name = entity.Name;
            user.Email = entity.Email;
            user.Password = entity.Password;
            _context.SaveChanges();
            return user;
        }
    }
}
