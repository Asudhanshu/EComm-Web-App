using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        [Key]
        [Required]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShoppingCartId { get; set; }



        [DataType("int")]
        public int ProductId { get; set; }



        [DataType("varchar(50)")]
        [StringLength(50)]
        [Required]
        public string ProductName { get; set; }



        [DataType("int")]
        public int ProductPrice { get; set; }




        [DataType("int")]
        public int ProductQty { get; set; }



        [DataType("varchar(50)")]
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }
    }
}