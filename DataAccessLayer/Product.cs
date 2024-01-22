using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccessLayer
{
    [Table("Product")]
    public class Product
    {
        [Key][Required][Column(TypeName = "int")][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [DataType("varchar(50)")][StringLength(50)][Required]
        public string ProductName { get; set; }
        [DataType("varchar(50)")][StringLength(30)]
        public string ProductBrand { get; set; }
        [DataType("varchar(100)")][StringLength(100)]
        public string ProductDescription { get; set; }
        [DataType("int")]
        public int ProductPrice { get; set; }
        [DataType("int")]
        public int ProductQty { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [DataType("DateTime")][StringLength(50)][Required]
        public DateTime CreatedOn { get; set; }
        [DataType("DateTime")][StringLength(50)][Required]
        public DateTime ModifiedOn { get; set; }
        [DataType("varchar(50)")][StringLength(50)][Required]
        public string ModifiedBy { get; set; }
        [DataType("varchar(100)")][StringLength(100)][Required]
        public string Image { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
