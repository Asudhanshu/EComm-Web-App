using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    [Table("Order")]
    public class Order
    {
        [Key][Required][Column(TypeName = "int")][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [DataType("varchar(50)")][StringLength(50)][Required]
        public string UserName { get; set; }
        [DataType("int")]
        public int TotalPrice { get; set; }
        [DataType("DateTime")][StringLength(50)][Required]
        public DateTime OrderDate { get; set; }
    }
}
