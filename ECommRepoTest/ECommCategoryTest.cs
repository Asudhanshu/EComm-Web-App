using ECommRepo;
using ECommRepo.Models;
using ECommRepo.Repository;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using AutoMapper;

namespace EcommRepoTest
{
    public class ECommCategoryTest
    {
        ECommDbContext context;
        ICategoryRepo categoryRepository;
        IMapper _mapper;
        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ECommDbContext>()
           .UseInMemoryDatabase(databaseName: "EcommDatabase").Options;
            context = new ECommDbContext(options);
            // Insert seed data into the database using one instance of the context
            categoryRepository = new CategoryRepo(context,_mapper);
            context.category.Add(new Category { CategoryId = 1, CategoryName = "IPhone11", CategoryDescription = "IPhone" });
            context.category.Add(new Category { CategoryId = 2, CategoryName = "IPhone12", CategoryDescription = "IPhone12" });
            context.SaveChanges();
        }
        [OneTimeTearDown]
        public void CleanUp()
        {

            context.Dispose();
        }

        [Test]
        [Order(1)]
        public void GetCategoryTest()
        {
            var categories = categoryRepository.GetCategoryByList();
            Assert.AreEqual(2, categories.Count);
            Assert.AreEqual("IPhone11", categories[0].CategoryName);
            Assert.AreEqual("IPhone12", categories[1].CategoryName);
        }


        [Test]
        [Order(2)]
        public void UpdateCategoryTest()
        {
            var categoryData = context.category.Find(1);
            var categoryModel = new CategoryModel();
            PropertyCopy<Category, CategoryModel>.Copy(categoryData, categoryModel);
            //_mapper.Map(categoryData, categoryModel);
            categoryModel.CategoryName = "Samsung";
            categoryRepository.UpdateCategoryByList(categoryModel);
            var updatedbookData = context.category.Find(1);
            Assert.AreEqual("Samsung", updatedbookData.CategoryName);
        }

        [Test]
        [Order(3)]
        public void DeleteCategoryTest()
        {
            categoryRepository.DeleteCategory(2);
            var categories = categoryRepository.GetCategoryByList();
            Assert.AreEqual(1, categories.Count);
        }
        [Test]
        [Order(4)]
        public void CreateCategoryTest()
        {
            CategoryModel categoryModel = new CategoryModel() { CategoryId = 2, CategoryName = "Category3", CategoryDescription = "IPhone12" };
            var categoryAdd = categoryRepository.CreateTest(categoryModel);
            Assert.AreEqual("Category3", categoryAdd.CategoryName);
        }
    }
}