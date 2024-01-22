using ECommRepo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommRepo.Repository
{
    public interface IUsersRepo
    {
        Task<UsersModel> GetUser(int id);
        List<UsersModel> GetUserByList();
        Task<List<UsersModel>> GetUsers();
        Task<UsersModel> UpdateUser(UsersModel usersModel);
        void UpdateUserByList(UsersModel userModel);
    }
}