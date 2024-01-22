using Microsoft.AspNetCore.Http;
using System;
namespace ECommRepo.Models
{
    /// <summary>
    /// Product Model is used to map the data from Data Access Layer Product Model
    /// </summary>
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int ProductQty { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
