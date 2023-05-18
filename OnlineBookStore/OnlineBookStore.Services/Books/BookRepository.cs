using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineBookStore.Core.Entities;
using OnlineBookStore.Data.Contexts;
using OnlineBookStore.Services.Extensions;

namespace OnlineBookStore.Services.Books;

public class BookRepository : IBookRepository
{
    private readonly BookDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public BookRepository(BookDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<Book> GetBookByIdAsync(int bookId)
    {
        return await _context.Set<Book>().FindAsync(bookId);
    }

    public async Task<Book> GetCachedBookByIdAsync(int bookId)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"book.by-id.{bookId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetBookByIdAsync(bookId);
            });
    }

    public async Task<IList<Book>> GetBooksAsync(string keyword = "", CancellationToken cancellationToken = default)
    {
        return await _context.Set<Book>()
            .WhereIf(!string.IsNullOrWhiteSpace(keyword), b => b.Title.ToLower().Contains(keyword.ToLower()) || b.Description.ToLower().Contains(keyword.ToLower()))
            .OrderBy(a => a.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AddOrUpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        if (book.Id > 0)
        {
            _context.Books.Update(book);
            _memoryCache.Remove($"book.by-id.{book.Id}");
        }
        else
        {
            _context.Books.Add(book);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteBookAsync(int bookId, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Where(x => x.Id == bookId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<bool> SetImageUrlAsync(int bookId, string imageUrl, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Where(x => x.Id == bookId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(a => a.Cover, a => imageUrl),
                cancellationToken) > 0;
    }
    
    public async Task<bool> SetFileUrlAsync(int bookId, string fileUrl, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Where(x => x.Id == bookId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(a => a.File, a => fileUrl),
                cancellationToken) > 0;
    }
}