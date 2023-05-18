using OnlineBookStore.Core.Entities;
using OnlineBookStore.Data.Contexts;

namespace OnlineBookStore.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BookDbContext _dbContext;

        public DataSeeder(BookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Books.Any())
            {
                return;
            }

            AddAdmin();

            var authors = AddAuthors();
            var categories = AddCategories();
            var books = AddBooks(authors, categories);
        }

        private IList<Book> AddBooks(IList<Author> authors, IList<Category> categories)
        {
            var books = new List<Book>()
            {
                new Book()
                {
                    Title = "Web Development Recipes 2nd Edition",
                    Description = "Web Development Recipes",
                    Cover = "64551839cc22c9.78911467.jpg",
                    File = "645515b755bbe2.18723160.pdf",
                    Author = authors[1],
                    Category = categories[0]
                },
                new Book()
                {
                    Title = "JavaScript from Beginner to Professional",
                    Description = "JavaScript from Beginner to Professional: Learn JavaScript quickly by building fun, interactive, and dynamic web apps, games, and pages",
                    Cover = "6455193fd04039.69162472.jpg",
                    File = "6455193fd06334.88976128.pdf",
                    Author = authors[2],
                    Category = categories[1]
                },
                new Book()
                {
                    Title = "C# 10 and .NET 6 – Modern Cross-Platform Development",
                    Description = "C# 10 and .NET 6 – Modern Cross-Platform Development: Build apps, websites, and services with ASP.NET Core 6, Blazor, and EF Core 6 using Visual Studio 2022 and Visual Studio Code, 6th Edition",
                    Cover = "64551a38cbe3c4.03257030.jpg",
                    File = "64551a38cc1793.44828931.pdf",
                    Author = authors[3],
                    Category = categories[1]
                }
            };

            _dbContext.Books.AddRange(books);
            _dbContext.SaveChanges();

            return books;
        }

        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new Category() { Name = "Web" },
                new Category() { Name = "Programming" }
            };

            _dbContext.Categories.AddRange(categories);
            _dbContext.SaveChanges();

            return categories;
        }

        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
            {
                new Author() { Name = "Adam Freeman" },
                new Author() { Name = "Chris Johnson" },
                new Author() { Name = "Laurence Lars Svekis" },
                new Author() { Name = "Mark J. Price" }
            };

            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();

            return authors;
        }

        private void AddAdmin()
        {
            var admin = new Admin
            {
                FullName = "Nguyen Vy",
                Email = "nguyenvy@email.com",
                Password = @"$2y$10$Nqq/y251QX2Ccvb1Ax7hUuMqQSkG3yRLCxN2KPdetnSP3oaXVH70a",
            };

            _dbContext.Admin.Add(admin);
            _dbContext.SaveChanges();
        }
    }
}
