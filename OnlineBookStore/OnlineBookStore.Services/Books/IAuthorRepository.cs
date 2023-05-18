using OnlineBookStore.Core.Constants;
using OnlineBookStore.Core.DTO;
using OnlineBookStore.Core.Entities;

namespace OnlineBookStore.Services.Books;

public interface IAuthorRepository
{
    Task<Author> GetAuthorByIdAsync(int authorId);

    Task<Author> GetCachedAuthorByIdAsync(int authorId);

    Task<IList<AuthorItem>> GetAuthorsAsync(string name = "", CancellationToken cancellationToken = default);

    Task<IList<Book>> GetBooksAsync<T>(BookQuery condition, Func<IQueryable<Book>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateAsync(Author author, CancellationToken cancellationToken = default);

    Task<bool> DeleteAuthorAsync(int authorId, CancellationToken cancellationToken = default);
}