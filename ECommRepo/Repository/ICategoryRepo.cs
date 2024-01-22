using ECommRepo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommRepo.Repository
{
    /// <summary>
    /// ICategoryRepo Interface is the interface in Business Logic Layer to fetch the data from DataAccess Layer and pass it to ECommAPI
    /// </summary>
    public interface ICategoryRepo
    {
        Task<CategoryModel> AddCategory(CategoryModel book);
        Task<CategoryModel> GetCategory(int id);
        Task DeleteCategory(int id);
        Task<List<CategoryModel>> GetCategories(int pg=1);
        Task<CategoryModel> UpdateCategory(CategoryModel bookModel);
        Task<int> GetCategoryCount();
        // function for unit test case
        List<CategoryModel> GetCategoryByList();
        void UpdateCategoryByList(CategoryModel categoryCategory);
        CategoryModel CreateTest(CategoryModel categoryModel);
    }
}