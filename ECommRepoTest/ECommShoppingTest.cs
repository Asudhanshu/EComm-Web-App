using ECommRepo;
using ECommRepo.Models;
using ECommRepo.Repository;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using AutoMapper;

namespace EcommRepoTest
{
    public class ECommShoppingTest
    {
        ECommDbContext context;
        IShoppingRepo shoppingRepository;
        IMapper _mapper;
        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ECommDbContext>()
           .UseInMemoryDatabase(databaseName: "EcommDatabase").Options;
            context = new ECommDbContext(options);
            // Insert seed data into the database using one instance of the context
            shoppingRepository = new ShoppingRepo(context,_mapper);
            context.ShoppingCart.Add(new ShoppingCart { ShoppingCartId = 1, ProductName = "IPhone11", ProductQty = 4, ProductId=1 , ProductPrice=100, UserName="Sudhanshu" });
            context.ShoppingCart.Add(new ShoppingCart { ShoppingCartId = 2, ProductName = "IPhone13", ProductQty = 3, ProductId = 2, ProductPrice = 200, UserName = "Sudhanshu" });
            context.SaveChanges();
        }
        [OneTimeTearDown]
        public void CleanUp()
        {

            context.Dispose();
        }

        [Test]
        [Order(1)]
        public void GetShoppingTest()
        {
            var shopping = shoppingRepository.GetShoppingByList();
            Assert.AreEqual(2, shopping.Count);
            Assert.AreEqual("IPhone11", shopping[0].ProductName);
            Assert.AreEqual("IPhone13", shopping[1].ProductName);
        }

        [Test]
        [Order(2)]
        public void UpdateShoppingTest()
        {
            var shoppingCart = context.ShoppingCart.Find(1);
            var shoppingCartModel = new ShoppingCartModel();
            PropertyCopy<ShoppingCart, ShoppingCartModel>.Copy(shoppingCart, shoppingCartModel);
            //_mapper.Map(shoppingCart, shoppingCartModel);
            shoppingCartModel.ProductName = "Samsung";
            shoppingRepository.UpdateShoppingByList(shoppingCartModel);
            var updatedShoppingData = context.ShoppingCart.Find(1);
            Assert.AreEqual("Samsung", updatedShoppingData.ProductName);
        }

        [Test]
        [Order(3)]
        public void CreateShoppingTest()
        {
            var shoppingModel = new ShoppingCartModel() { ShoppingCartId = 3, ProductName = "Product3", ProductQty = 4, ProductId = 3, ProductPrice = 100, UserName = "Sudhanshu" };
            var shoppingAdd = shoppingRepository.CreateTest(shoppingModel);
            Assert.AreEqual("Product3", shoppingAdd.ProductName);
        }
    }
}