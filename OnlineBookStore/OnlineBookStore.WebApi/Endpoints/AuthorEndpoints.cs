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
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
                .WithName("GetAuthors")
                .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
                .WithName("GetAuthorById")
                .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapGet("/{id:int}/books", GetBooksByAuthorId)
                .WithName("GetBooksByAuthorId")
                .Produces<ApiResponse<Book>>();

            routeGroupBuilder.MapPost("/", AddAuthor)
                .WithName("AddNewAuthor")
                .Produces(401)
                .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
                .WithName("UpdateAnAuthor")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
                .WithName("DeleteAnAuthor")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetAuthors([AsParameters] AuthorFilterModel model, IAuthorRepository authorRepository)
        {
            var authorsList = await authorRepository.GetAuthorsAsync(model.Name);
            return Results.Ok(ApiResponse.Success(authorsList));
        }

        private static async Task<IResult> GetAuthorDetails(int id, IAuthorRepository authorRepository, IMapper mapper)
        {
            var author = await authorRepository.GetAuthorByIdAsync(id);
            return author == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
        }

        private static async Task<IResult> GetBooksByAuthorId(int id, IAuthorRepository authorRepository)
        {
            var bookQuery = new BookQuery()
            {
                AuthorId = id
            };

            var booksList = authorRepository.GetBooksAsync(bookQuery, authors => authors.ProjectToType<Book>());

            return Results.Ok(ApiResponse.Success(booksList));
        }

        private static async Task<IResult> AddAuthor(AuthorEditModel model, IAuthorRepository authorRepository, IMapper mapper)
        {
            var author = mapper.Map<Author>(model);
            await authorRepository.AddOrUpdateAsync(author);

            //return Results.CreatedAtRoute("GetAuthorById", new { author.Id }, mapper.Map<AuthorItem>(author));
            return Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateAuthor(int id, AuthorEditModel model, IAuthorRepository authorRepository, IMapper mapper)
        {
            var author = mapper.Map<Author>(model);
            author.Id = id;

            //return await authorRepository.AddOrUpdateAsync(author) ? Results.NoContent() : Results.NotFound();
            return await authorRepository.AddOrUpdateAsync(author) ? Results.Ok(ApiResponse.Success("Author is updated", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
        }

        private static async Task<IResult> DeleteAuthor(int id, IAuthorRepository authorRepository)
        {
            //return await authorRepository.DeleteAuthorAsync(id) ? Results.NoContent() : Results.NotFound($"Could not find author with id = {id}");
            return await authorRepository.DeleteAuthorAsync(id) ? Results.Ok(ApiResponse.Success("Author is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
        }
    }
}
