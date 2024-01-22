using System.ComponentModel.DataAnnotations;


namespace ECommAPI.Models
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
