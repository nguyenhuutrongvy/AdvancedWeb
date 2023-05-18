using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBookStore.Core.Constants;
using OnlineBookStore.Core.DTO;
using OnlineBookStore.Core.Entities;
using OnlineBookStore.Services.Books;
using OnlineBookStore.Services.Media;
using OnlineBookStore.WebApi.Models;
using System.Net;

namespace OnlineBookStore.WebApi.Endpoints
{
    public static class BookEndpoints
    {
        public static WebApplication MapBookEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/books");

            routeGroupBuilder.MapGet("/", GetBooks)
                .WithName("GetBooks")
                .Produces<ApiResponse<Book>>();

            routeGroupBuilder.MapGet("/{id:int}", GetBookDetails)
                .WithName("GetBookById")
                .Produces<ApiResponse<Book>>()
                .Produces(404);

            routeGroupBuilder.MapGet("/get-books-filter", GetFilteredBooks)
                .WithName("GetFilteredBook")
                .Produces<ApiResponse<Book>>();

            routeGroupBuilder.MapPost("/", AddNewBook)
                .WithName("AddNewBook")
                /*.Accepts<BookEditModel>("multipart/form-data")*/
                .Produces(401)
                .Produces<ApiResponse<Book>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateBook)
                .WithName("UpdateABook")
                .Produces(204)
                .Produces(400)
                .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteBook)
                .WithName("DeleteABook")
                .Produces(204)
                .Produces(404);

            routeGroupBuilder.MapPost("/{id:int}/picture", SetBookPicture)
                .WithName("SetBookPicture")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces<ApiResponse<string>>()
                .Produces(400);

            routeGroupBuilder.MapPost("/{id:int}/file", SetBookFile)
                .WithName("SetBookFile")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces<ApiResponse<string>>()
                .Produces(400);

            return app;
        }

        private static async Task<IResult> GetBooks([AsParameters] BookFilterModel model, IBookRepository bookRepository)
        {
            BookQuery query = new BookQuery
            {
                Keyword = model.Keyword
            };

            var booksList = await bookRepository.GetBooksAsync(model.Keyword);

            return Results.Ok(ApiResponse.Success(booksList));
        }

        private static async Task<IResult> GetBookDetails(int id, IBookRepository bookRepository, IMapper mapper)
        {
            var book = await bookRepository.GetCachedBookByIdAsync(id);
            //return book == null ? Results.NotFound($"Không tìm thấy sách có mã số {id}") : Results.Ok(mapper.Map<Book>(book));
            return book == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy sách có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<Book>(book)));
        }

        private static async Task<IResult> SetBookPicture(int id, IFormFile imageFile, IBookRepository bookRepository, IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(),
                imageFile.FileName,
                imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                //return Results.BadRequest("Không lưu được tập tin");
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await bookRepository.SetImageUrlAsync(id, imageUrl.Split('/').Last());

            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> SetBookFile(int id, IFormFile fileFile, IBookRepository bookRepository, IMediaManager mediaManager)
        {
            var fileUrl = await mediaManager.SaveFileAsync(
                fileFile.OpenReadStream(),
                fileFile.FileName,
                fileFile.ContentType);

            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                //return Results.BadRequest("Không lưu được tập tin");
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await bookRepository.SetFileUrlAsync(id, fileUrl.Split('/').Last());

            return Results.Ok(ApiResponse.Success(fileUrl));
        }

        private static async Task<IResult> UpdateBook(int id, BookDto model, IBookRepository bookRepository, IMapper mapper)
        {
            var book = mapper.Map<Book>(model);
            book.Id = id;

            //return await bookRepository.AddOrUpdateAsync(book) ? Results.NoContent() : Results.NotFound();
            return await bookRepository.AddOrUpdateAsync(book) ? Results.Ok(ApiResponse.Success("Book is updated", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find book"));
        }

        private static async Task<IResult> DeleteBook(int id, IBookRepository bookRepository)
        {
            //return await bookRepository.DeleteBookAsync(id) ? Results.NoContent() : Results.NotFound($"Could not find book with id = {id}");
            return await bookRepository.DeleteBookAsync(id) ? Results.Ok(ApiResponse.Success("Book is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find book"));
        }

        private static async Task<IResult> GetFilteredBooks([AsParameters] BookFilterModel model, IBookRepository bookRepository)
        {
            var bookQuery = new BookQuery()
            {
                Keyword = model.Keyword
            };

            var booksList = await bookRepository.GetBooksAsync(bookQuery.Keyword);

            return Results.Ok(ApiResponse.Success(booksList));
        }

        /*private static async Task<IResult> AddBook(HttpContext context, IBookRepository bookRepository, IMapper mapper, IMediaManager mediaManager)
        {
            var model = await BookEditModel.BindAsync(context);

            var book = new Book()
            {
                Id = 0
            };

            book.Title = model.Title;
            book.AuthorId = model.AuthorId;
            book.CategoryId = model.CategoryId;
            book.Description = model.Description;

            if (model.Cover?.Length > 0)
            {
                string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/",
                uploadedCoverPath = await mediaManager.SaveFileAsync(
                    model.Cover.OpenReadStream(),
                    model.Cover.FileName,
                    model.Cover.ContentType);
                if (!string.IsNullOrWhiteSpace(uploadedCoverPath))
                {
                    book.Cover = uploadedCoverPath.Split('/').Last();
                }
            }
            if (model.File?.Length > 0)
            {
                string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/",
                uploadedFilePath = await mediaManager.SaveFileAsync(
                    model.File.OpenReadStream(),
                    model.File.FileName,
                    model.File.ContentType);
                if (!string.IsNullOrWhiteSpace(uploadedFilePath))
                {
                    book.File = uploadedFilePath.Split('/').Last();
                }
            }
            await bookRepository.AddOrUpdateAsync(book);
            return Results.Ok(ApiResponse.Success(
            mapper.Map<Book>(book), HttpStatusCode.Created));
        }*/

        private static async Task<IResult> AddNewBook(BookEditModel model, IBookRepository bookRepository, IMapper mapper)
        {
            var book = mapper.Map<Book>(model);
            await bookRepository.AddOrUpdateAsync(book);

            return Results.Ok(ApiResponse.Success(mapper.Map<Book>(book), HttpStatusCode.Created));
        }
    }
}
