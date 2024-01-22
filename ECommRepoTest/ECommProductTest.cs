using ECommRepo;
using ECommRepo.Models;
using ECommRepo.Repository;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using AutoMapper;

namespace EcommRepoTest
{
    public class ECommProductTest
    {
        ECommDbContext context;
        IMapper _mapper;
        IProductRepo productRepository;
        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ECommDbContext>()
           .UseInMemoryDatabase(databaseName: "EcommDatabase")
           .Options;
            context = new ECommDbContext(options);
            // Insert seed data into the database using one instance of the context
            productRepository = new ProductRepo(context,_mapper);
            context.product.Add(new Product { ProductId = 1, ProductName = "IPhone11", ProductDescription = "IPhone", ProductBrand = "Apple", ProductPrice = 200000, ProductQty=10 });
            context.product.Add(new Product { ProductId = 2, ProductName = "IPhone12", ProductDescription = "IPhone12", ProductBrand = "AppleIPhone", ProductPrice = 200100, ProductQty = 6 });
            context.SaveChanges();
        }
        [OneTimeTearDown]
        public void CleanUp()
        {

            context.Dispose();
        }

        [Test]
        [Order(1)]
        public void GetProductTest()
        {
            var products = productRepository.GetProductsByList();
            Assert.AreEqual(2, products.Count);
            Assert.AreEqual("IPhone11", products[0].ProductName);
            Assert.AreEqual("IPhone12", products[1].ProductName);
        }


        [Test]
        [Order(2)]
        public void UpdateProductTest()
        {
            var productData = context.product.Find(1);
            var productModel = new ProductModel();
            PropertyCopy<Product, ProductModel>.Copy(productData, productModel);
            //_mapper.Map(productData, productModel);
            productModel.ProductName = "Samsung";
            productRepository.UpdateProductByList(productModel);
            var updatedbookData = context.product.Find(1);
            Assert.AreEqual("Samsung", updatedbookData.ProductName);
        }

        [Test]
        [Order(3)]
        public void DeleteProductTest()
        {
            productRepository.DeleteProduct(2);
            var products = productRepository.GetProductsByList();
            Assert.AreEqual(1, products.Count);
        }
        [Test]
        [Order(4)]
        public void CreateProductTest()
        {
            var productModel = new ProductModel() { ProductId = 2, ProductName = "Product3", ProductDescription = "IPhone12", ProductBrand = "AppleIPhone", ProductPrice = 200100, ProductQty = 6 };
            var productAdd = productRepository.CreateTest(productModel);
            Assert.AreEqual("Product3", productAdd.ProductName);
        }
    }
}