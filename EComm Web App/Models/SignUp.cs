using System.ComponentModel.DataAnnotations;
namespace EComm_Web_App.Models
{
    /// <summary>
    /// Signup Class is used for Registering user to log into SHAPT EComm
    /// </summary>
    public class SignUp
    {
        //Properties and attributes
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Compare(nameof(Password), ErrorMessage = "Password didn't match")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
