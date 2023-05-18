using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineBookStore.Core.Constants;
using OnlineBookStore.Core.DTO;
using OnlineBookStore.Core.Entities;
using OnlineBookStore.Data.Contexts;
using OnlineBookStore.Services.Extensions;

namespace OnlineBookStore.Services.Books;

public class AuthorRepository : IAuthorRepository
{
    private readonly BookDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public AuthorRepository(BookDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<Author> GetAuthorByIdAsync(int authorId)
    {
        return await _context.Set<Author>().FindAsync(authorId);
    }

    public async Task<Author> GetCachedAuthorByIdAsync(int authorId)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"author.by-id.{authorId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetAuthorByIdAsync(authorId);
            });
    }

    public async Task<IList<AuthorItem>> GetAuthorsAsync(string name = "", CancellationToken cancellationToken = default)
    {
        return await _context.Set<Author>()
            .WhereIf(!string.IsNullOrWhiteSpace(name), a => a.Name.ToLower().Contains(name.ToLower()))
            .OrderByDescending(a => a.Id)
            .ThenBy(a => a.Name)
            .Select(a => new AuthorItem()
            {
                Id = a.Id,
                Name = a.Name,
                BookCount = a.Books.Count()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<Book>> GetBooksAsync<T>(BookQuery condition, Func<IQueryable<Book>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Book>()
            .WhereIf(condition.AuthorId != null, b => b.AuthorId == condition.AuthorId)
            .OrderByDescending(b => b.Id)
            .ThenBy(b => b.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AddOrUpdateAsync(Author author, CancellationToken cancellationToken = default)
    {
        if (author.Id > 0)
        {
            _context.Authors.Update(author);
            _memoryCache.Remove($"author.by-id.{author.Id}");
        }
        else
        {
            _context.Authors.Add(author);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAuthorAsync(int authorId, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(x => x.Id == authorId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    private IQueryable<Book> FilterBooks(BookQuery condition)
    {
        return _context.Set<Book>()
            .Include(x => x.Author)
            .Include(x => x.Category)
            .WhereIf(condition.AuthorId > 0, x => x.AuthorId == condition.AuthorId)
            .WhereIf(condition.CategoryId > 0, x => x.CategoryId == condition.CategoryId)
            .WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), x => x.Title.ToLower().Contains(condition.Keyword.ToLower()) ||
                                                                         x.Description.ToLower().Contains(condition.Keyword.ToLower()) ||
                                                                         x.Category.Name.ToLower().Contains(condition.Keyword.ToLower()) ||
                                                                         x.Author.Name.ToLower().Contains(condition.Keyword.ToLower()));
    }
}