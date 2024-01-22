using ECommRepo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ECommRepo.Repository
{
    /// <summary>
    /// IProductRepo Interface is the Interface in Business Logic Layer to fetch the data from DataAccess Layer and pass it to ECommAPI
    /// </summary>
    public interface IProductRepo
    {
        Task<ProductModel> AddProduct(ProductModel productModel);
        Task DeleteProduct(int id);
        Task<ProductModel> GetProduct(int id);
        Task<List<ProductModel>> GetProducts(int pg=1);
        List<ProductModel> GetProductsByList();
        Task<ProductModel> UpdateProduct(ProductModel productModel);
        void UpdateProductByList(ProductModel productModel);
        Task<List<CategoryModel>> GetCategory();
        ProductModel CreateTest(ProductModel productModel);
        Task<int> GetProductCount();

        
        Task<List<ProductModel>> GetProductsList();
    }
}