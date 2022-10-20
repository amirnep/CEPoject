using Domain.Models.Entities.User;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ShopDbContext _context;

        public UserRepository(ShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int IDUser)
        {
            return _context.Users.Find(IDUser);
        }

        public void DeleteUser(int IDUser)
        {
            var user = _context.Users.Find(IDUser);
            _context.Users.Remove(user);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
