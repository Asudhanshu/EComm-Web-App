using System.ComponentModel.DataAnnotations;
namespace EComm_Web_App.Models
{
    public class Login
    {
        [Required][DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required][DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RemeberMe { get; set; }
    }
}
