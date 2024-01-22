using System;
namespace ECommRepo.Models
{
    /// <summary>
    /// Category Model is used to map the data from Data Access Layer Category Model
    /// </summary>
    public class CategoryModel
    {
        //Properties
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
