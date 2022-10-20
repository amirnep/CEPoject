using Domain.Models.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(int IDUser);
        void DeleteUser(int IDUser);
        void Save();
    }
}
