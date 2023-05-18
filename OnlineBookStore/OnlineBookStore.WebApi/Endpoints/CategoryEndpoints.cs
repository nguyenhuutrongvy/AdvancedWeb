using FluentValidation;
using Mapster;
using MapsterMapper;
using OnlineBookStore.Core.Constants;
using OnlineBookStore.Core.DTO;
using OnlineBookStore.Core.Entities;
using OnlineBookStore.Services.Books;
using OnlineBookStore.WebApi.Models;
using System.Net;

namespace OnlineBookStore.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<ApiResponse<CategoryItem>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                .WithName("GetCategoryById")
                .Produces<ApiResponse<CategoryItem>>();

            routeGroupBuilder.MapGet("/{id:int}/books", GetBooksByCategoryId)
                .WithName("GetBooksByCategoryId")
                .Produces<ApiResponse<Book>>();

            routeGroupBuilder.MapPost("/", AddCategory)
                .WithName("AddNewCategory")
                .Produces(401)
                .Produces<ApiResponse<CategoryItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
                .WithName("UpdateAnCategory")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
                .WithName("DeleteAnCategory")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetCategories([AsParameters] CategoryFilterModel model, ICategoryRepository authorRepository)
        {
            var categoriesList = await authorRepository.GetCategoriesAsync(model.Name);
            return Results.Ok(ApiResponse.Success(categoriesList));
        }

        private static async Task<IResult> GetCategoryDetails(int id, ICategoryRepository authorRepository, IMapper mapper)
        {
            var author = await authorRepository.GetCachedCategoryByIdAsync(id);
            return author == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(author)));
        }

        private static async Task<IResult> GetBooksByCategoryId(int id, ICategoryRepository authorRepository)
        {
            var bookQuery = new BookQuery()
            {
                CategoryId = id
            };

            var booksList = authorRepository.GetBooksAsync(bookQuery, categories => categories.ProjectToType<Book>());

            return Results.Ok(ApiResponse.Success(booksList));
        }

        private static async Task<IResult> AddCategory(CategoryEditModel model, ICategoryRepository authorRepository, IMapper mapper)
        {
            var author = mapper.Map<Category>(model);
            await authorRepository.AddOrUpdateAsync(author);

            //return Results.CreatedAtRoute("GetCategoryById", new { author.Id }, mapper.Map<CategoryItem>(author));
            return Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(author), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateCategory(int id, CategoryEditModel model, ICategoryRepository authorRepository, IMapper mapper)
        {
            var author = mapper.Map<Category>(model);
            author.Id = id;

            //return await authorRepository.AddOrUpdateAsync(author) ? Results.NoContent() : Results.NotFound();
            return await authorRepository.AddOrUpdateAsync(author) ? Results.Ok(ApiResponse.Success("Category is updated", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
        }

        private static async Task<IResult> DeleteCategory(int id, ICategoryRepository authorRepository)
        {
            //return await authorRepository.DeleteCategoryAsync(id) ? Results.NoContent() : Results.NotFound($"Could not find author with id = {id}");
            return await authorRepository.DeleteCategoryAsync(id) ? Results.Ok(ApiResponse.Success("Category is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
        }
    }
}
