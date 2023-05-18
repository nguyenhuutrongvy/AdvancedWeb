using OnlineBookStore.Core.Entities;

namespace OnlineBookStore.Services.Books;

public interface IBookRepository
{
    Task<Book> GetBookByIdAsync(int bookId);

    Task<Book> GetCachedBookByIdAsync(int bookId);

    Task<IList<Book>> GetBooksAsync(string keyword = "", CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateAsync(Book book, CancellationToken cancellationToken = default);

    Task<bool> DeleteBookAsync(int bookId, CancellationToken cancellationToken = default);

    Task<bool> SetImageUrlAsync(int bookId, string imageUrl, CancellationToken cancellationToken = default);

    Task<bool> SetFileUrlAsync(int bookId, string fileUrl, CancellationToken cancellationToken = default);
}