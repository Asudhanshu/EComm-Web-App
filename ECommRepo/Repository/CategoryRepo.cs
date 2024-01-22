using AutoMapper;
using DataAccessLayer;
using EComm_Web_App.Models;
using ECommRepo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ECommRepo.Repository
{
    /// <summary>
    /// CategoryRepo Class is the class in Business Logic Layer to fetch the data from DataAccess Layer and pass it to ECommAPI
    /// </summary>
    public class CategoryRepo : ICategoryRepo
    {
        //Readonly Property
        private readonly ECommDbContext _context;
        private readonly IMapper _mapper;
        Category categoryData;
        /// <summary>
        /// Constructor to initialize the property
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public CategoryRepo(ECommDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// To get the list of categories from the Database and pass it to ECommApi Using Paging
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryModel>> GetCategories(int pg=1)
        {
            if (pg < 1)
            {
                pg = 1;
            }
            int pgSize = 5;
            int recsCount = _context.category.Count();
            var pager = new Pager(recsCount, pg, pgSize);
            int recSkip = (pg - 1) * pgSize;
            //Taking defined number of records from Database
            return await _context.category.Select(x => new CategoryModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                CategoryDescription = x.CategoryDescription
            }).OrderBy(x => x.CategoryId).Skip(recSkip).Take(pager.PageSize).ToListAsync();
        }
        /// <summary>
        /// To Get the total number of records present in the database
        /// </summary>
        /// <returns>int</returns>
        public async Task<int> GetCategoryCount()
        {
            var getCount = _context.category.Count();
            return getCount;
        }
        /// <summary>
        /// To get the category from the Database and pass it to ECommApi based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CategoryModel> GetCategory(int id)
        {
            var category = await _context.category.FindAsync(id);
            CategoryModel categoryModel = new CategoryModel();
            _mapper.Map(category, categoryModel);
            return categoryModel;
        }
        /// <summary>
        /// Add category is the method to add the data into database which we are getting from the API
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        public async Task<CategoryModel> AddCategory(CategoryModel categoryModel)
        {
            Category category = new Category();
            _mapper.Map(categoryModel, category);
            category.CreatedOn = DateTime.Now;
            category.ModifiedOn = DateTime.Now;
            category.ModifiedBy = "sudhanshu";
            _context.category.Add(category);
            await _context.SaveChangesAsync();
            return categoryModel;
        }
        /// <summary>
        /// UpdateCategory is the method used for updating the categorys into database
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        public async Task<CategoryModel> UpdateCategory(CategoryModel categoryModel)
        {
            Category category = new Category();
            _mapper.Map(categoryModel, category);
            category.ModifiedOn = DateTime.Now;
            category.ModifiedBy = "Sudhanshu";
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return categoryModel;

        }
        /// <summary>
        /// DeleteCategory Method is deleting the data based on the Id into databse from WebApi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCategory(int id)
        {
            var categoryModel = await _context.category.FindAsync(id);
            if (categoryModel == null)
            {
                return;
            }
            _context.category.Remove(categoryModel);
            await _context.SaveChangesAsync();
            return;
        }
        //Functions for Unit testing in CategoryRepositoryTest
        public List<CategoryModel> GetCategoryByList()
        {
            return _context.category.Select(categoryCategory =>
                new CategoryModel
                {
                    CategoryId = categoryCategory.CategoryId,
                    CategoryName = categoryCategory.CategoryName,
                    CategoryDescription = categoryCategory.CategoryDescription
                }
            ).ToList();
        }
        public void UpdateCategoryByList(CategoryModel categoryCategory)
        {
            try
            {
                categoryData = _context.category.Find(categoryCategory.CategoryId);
                PropertyCopy<CategoryModel, Category>.Copy(categoryCategory, categoryData);
                _context.category.Update(categoryData);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public CategoryModel CreateTest(CategoryModel categoryModel)
        {
            Category category = new Category();
            PropertyCopy<CategoryModel, Category>.Copy(categoryModel, category);
            category.CreatedOn = DateTime.Now;
            category.ModifiedOn = DateTime.Now;
            category.ModifiedBy = "Harsh";
            _context.category.Add(category);
            _context.SaveChanges();
            return categoryModel;
        }
    }
}
