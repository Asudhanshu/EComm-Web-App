using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccessLayer
{
    [Table("Category")]
    public class Category
    {
        [Key][Required][Column(TypeName = "int")][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [DataType("varchar(50)")][StringLength(50)][Required]
        public string CategoryName { get; set; }
        [DataType("varchar(50)")][StringLength(50)]
        public string CategoryDescription { get; set; }
        public ICollection<Product> Product { get; set; }
        [DataType("DateTime")][StringLength(50)][Required]
        public DateTime CreatedOn { get; set; }
        [DataType("DateTime")][StringLength(50)][Required]
        public DateTime ModifiedOn { get; set; }
        [DataType("varchar(50)")][StringLength(50)][Required]
        public string ModifiedBy { get; set; }
    }
}
