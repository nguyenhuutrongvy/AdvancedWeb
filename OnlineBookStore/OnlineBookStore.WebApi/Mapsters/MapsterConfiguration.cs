using Mapster;
using OnlineBookStore.Core.DTO;
using OnlineBookStore.Core.Entities;
using OnlineBookStore.WebApi.Models;

namespace OnlineBookStore.WebApi.Mapsters
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Author, AuthorDto>();

            config.NewConfig<Author, AuthorItem>()
                .Map(dest => dest.BookCount, src => src.Books == null ? 0 : src.Books.Count);

            config.NewConfig<AuthorEditModel, Author>();

            config.NewConfig<Category, CategoryDto>();

            config.NewConfig<Category, CategoryItem>()
                .Map(dest => dest.BookCount, src => src.Books == null ? 0 : src.Books.Count);

            config.NewConfig<Book, BookDto>();

            config.NewConfig<BookEditModel, Book>();
        }
    }
}
