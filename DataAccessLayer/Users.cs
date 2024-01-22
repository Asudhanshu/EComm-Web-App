using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccessLayer
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [Required]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [DataType("varchar(450)")]
        [StringLength(450)]
        [Required]
        public string AspUserId { get; set; }
        [DataType("varchar(50)")]
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }
        [DataType("varchar(50)")]
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }
        [DataType("varchar(50)")]
        [StringLength(50)]
        [Required]
        public string Address { get; set; }
        [DataType("varchar(50)")]
        [StringLength(50)]
        [Required]
        public string Mobile { get; set; }
        [DataType("varchar(50)")]
        [StringLength(50)]
        [Required]
        public string Email { get; set; }
        [DataType("DateTime")]
        [StringLength(50)]
        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
