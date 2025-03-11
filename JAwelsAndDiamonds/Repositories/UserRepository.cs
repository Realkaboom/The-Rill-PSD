using System;
using System.Linq;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(JAwelsAndDiamondsEntities context) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(u => u.Email == email);
        }

        public User GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.Username == username);
        }

        public User ValidateUser(string email, string password)
        {
            return _dbSet.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public bool ChangePassword(int userId, string newPassword)
        {
            try
            {
                var user = GetById(userId);
                if (user == null)
                    return false;

                user.Password = newPassword;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}