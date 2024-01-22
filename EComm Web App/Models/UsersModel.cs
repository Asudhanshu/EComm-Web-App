using System;
namespace EComm_Web_App.Models
{
    public class UsersModel
    {
        public int UserId { get; set; }
        public string AspUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
