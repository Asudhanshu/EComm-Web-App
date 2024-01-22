using ECommRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using EComm_Web_App.Models;
using AutoMapper;

namespace ECommRepo.Repository
{
    /// <summary>
    /// ProductRepo Class is the class in Business Logic Layer to fetch the data from DataAccess Layer and pass it to ECommAPI
    /// </summary>
    public class ProductRepo : IProductRepo
    {
        private readonly ECommDbContext _context;
        private readonly IMapper _mapper;
        /// <summary>
        /// Inititalizing the Readonly properties through constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public ProductRepo(ECommDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// This method is used to get list of products based on the paging through Database
        /// </summary>
        /// <returns>List of Product</returns>
        public async Task<List<ProductModel>> GetProducts(int pg)
        {
            if (pg < 1)
            {
                pg = 1;
            }
            int pgSize = 5;
            int recsCount = _context.product.Count();
            var pager = new Pager(recsCount, pg, pgSize);
            int recSkip = (pg - 1) * pgSize;
            //Taking defined number of records through database using paging
            List<ProductModel>list= await _context.product.Select(x => new ProductModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                ProductPrice = x.ProductPrice,
                ProductBrand = x.ProductBrand,
                ProductQty = x.ProductQty,
                Image = x.Image,
            }).OrderBy(x=>x.ProductId).Skip(recSkip).Take(pager.PageSize).ToListAsync();
            return list;
        }
        /// <summary>
        /// This method is used to get complete list of products from Database
        /// </summary>
        /// <returns>List of Product</returns>
        public async Task<List<ProductModel>> GetProductsList()
        {
            List<ProductModel> list = await _context.product.Select(x => new ProductModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                ProductPrice = x.ProductPrice,
                ProductBrand = x.ProductBrand,
                ProductQty = x.ProductQty
            }).ToListAsync();
            return list;
        }
        /// <summary>
        /// To get the total number of products available in the database
        /// </summary>
        /// <returns>int</returns>
        public async Task<int> GetProductCount()
        {
            var getCount = _context.product.Count();
            return getCount;
        }

        /// <summary>
        /// This method is used to get the product based on the Id
        /// </summary>
        /// <returns>ProductModel</returns>
        public async Task<ProductModel> GetProduct(int id)
        {
            var product = await _context.product.FindAsync(id);
            ProductModel productModel = new ProductModel();
            _mapper.Map(product, productModel);
            return productModel;
        }
        /// <summary>
        /// To Add the product in the database
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        public async Task<ProductModel> AddProduct(ProductModel productModel)
        {
            Product product = new Product();
            _mapper.Map(productModel, product);
            product.CreatedOn = DateTime.Now;
            product.ModifiedOn = DateTime.Now;
            _context.product.Add(product);
            await _context.SaveChangesAsync();
            return productModel;
        }
        /// <summary>
        /// To delete the product based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProduct(int id)
        {
            var productModel = await _context.product.FindAsync(id);
            if (productModel == null)
            {
                return;
            }
            _context.product.Remove(productModel);
            await _context.SaveChangesAsync();
            return;
        }
        /// <summary>
        /// update the product
        /// </summary>
        /// <param name="productModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductModel> UpdateProduct(ProductModel productModel)
        {
            int id = productModel.ProductId;
            var products = await _context.product.FindAsync(id);
            products.ProductName = productModel.ProductName;
            products.ProductBrand = productModel.ProductBrand;
            products.ProductDescription = productModel.ProductDescription;
            products.ProductPrice = productModel.ProductPrice;
            products.ProductQty = productModel.ProductQty;
            products.CategoryId = productModel.CategoryId;
            products.ModifiedOn = DateTime.Now;
            products.ModifiedBy = productModel.ModifiedBy;
            try
            {
                _context.product.Update(products);  
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            await _context.SaveChangesAsync();
            return productModel;
        }
        /// <summary>
        /// To get the list of category present in the database
        /// </summary>
        /// <returns>List of Category</returns>
        public async Task<List<CategoryModel>> GetCategory()
        {
            return await _context.category.Select(x =>
            new CategoryModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            }).ToListAsync();
        }
        /// <summary>
        /// Below methods are used for the Nunit test
        /// for the testing purpose below four methods are returned
        /// </summary>
        /// <returns></returns>
        public List<ProductModel> GetProductsByList()
        {
            return _context.product.Select(x =>
            new ProductModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
            }
            ).ToList();
        }
        public void UpdateProductByList(ProductModel productModel)
        {
            Product productData;
            try
            {
                productData = _context.product.Find(productModel.ProductId);
                PropertyCopy<ProductModel, Product>.Copy(productModel, productData);
                _context.product.Update(productData);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public ProductModel CreateTest(ProductModel productModel)
        {
            Product product = new Product();
            PropertyCopy<ProductModel, Product>.Copy(productModel, product);
            product.CreatedOn = DateTime.Now;
            product.ModifiedOn = DateTime.Now;
            product.ModifiedBy = "Harsh";
            _context.product.Add(product);
            _context.SaveChanges();
            return productModel;
        }
    }
}