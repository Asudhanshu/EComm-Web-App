using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using ECommRepo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommRepo.Repository
{
    public class UsersRepo : IUsersRepo
    {
        private readonly ECommDbContext _context;
        Users userData;
        /// <summary>
        /// Constructor to initialize the property
        /// </summary>
        /// <param name="context"></param>
        public UsersRepo(ECommDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// To get the list of books from the Database and pass it to BookApi
        /// </summary>
        /// <returns></returns>
        public async Task<List<UsersModel>> GetUsers()
        {
            return await _context.users.Select(x => new UsersModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Mobile = x.Mobile,
                Address = x.Address
            }).ToListAsync();
        }
        /// <summary>
        /// To get the book from the Database and pass it to BookApi based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UsersModel> GetUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            UsersModel UsersModel = new UsersModel();
            PropertyCopy<Users, UsersModel>.Copy(user, UsersModel);
            return UsersModel;

        }
        /// <summary>
        /// UpdateCategory is the method used for updating the categorys into database
        /// </summary>
        /// <param name="UsersModel"></param>
        /// <returns></returns>
        public async Task<UsersModel> UpdateUser(UsersModel usersModel)
        {
            Users user = new Users();
            PropertyCopy<UsersModel, Users>.Copy(usersModel, user);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return usersModel;

        }

        //Functions for Unit testing in CategoryRepositoryTest
        public List<UsersModel> GetUserByList()
        {
            return _context.users.Select(x =>

                new UsersModel
                {
                    UserId = x.UserId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Mobile = x.Mobile,
                    Address = x.Address
                }
            ).ToList();

        }

        public void UpdateUserByList(UsersModel userModel)
        {
            try
            {
                userData = _context.users.Find(userModel.UserId);
                PropertyCopy<UsersModel, Users>.Copy(userModel, userData);
                _context.users.Update(userData);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
