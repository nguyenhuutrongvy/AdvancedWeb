using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineBookStore.Core.Constants;
using OnlineBookStore.Core.DTO;
using OnlineBookStore.Core.Entities;
using OnlineBookStore.Data.Contexts;
using OnlineBookStore.Services.Extensions;

namespace OnlineBookStore.Services.Books;

public class CategoryRepository : ICategoryRepository
{
    private readonly BookDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public CategoryRepository(BookDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<Category> GetCategoryByIdAsync(int authorId)
    {
        return await _context.Set<Category>().FindAsync(authorId);
    }

    public async Task<Category> GetCachedCategoryByIdAsync(int authorId)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"author.by-id.{authorId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetCategoryByIdAsync(authorId);
            });
    }

    public async Task<IList<CategoryItem>> GetCategoriesAsync(string name = "", CancellationToken cancellationToken = default)
    {
        return await _context.Set<Category>()
            .WhereIf(!string.IsNullOrWhiteSpace(name), a => a.Name.ToLower().Contains(name.ToLower()))
            .OrderByDescending(a => a.Id)
            .ThenBy(a => a.Name)
            .Select(a => new CategoryItem()
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
            .WhereIf(condition.CategoryId != null, b => b.CategoryId == condition.CategoryId)
            .OrderByDescending(b => b.Id)
            .ThenBy(b => b.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AddOrUpdateAsync(Category author, CancellationToken cancellationToken = default)
    {
        if (author.Id > 0)
        {
            _context.Categories.Update(author);
            _memoryCache.Remove($"author.by-id.{author.Id}");
        }
        else
        {
            _context.Categories.Add(author);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteCategoryAsync(int authorId, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(x => x.Id == authorId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    private IQueryable<Book> FilterBooks(BookQuery condition)
    {
        return _context.Set<Book>()
            .Include(x => x.Category)
            .Include(x => x.Category)
            .WhereIf(condition.CategoryId > 0, x => x.CategoryId == condition.CategoryId)
            .WhereIf(condition.CategoryId > 0, x => x.CategoryId == condition.CategoryId)
            .WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), x => x.Title.ToLower().Contains(condition.Keyword.ToLower()) ||
                                                                         x.Description.ToLower().Contains(condition.Keyword.ToLower()) ||
                                                                         x.Category.Name.ToLower().Contains(condition.Keyword.ToLower()) ||
                                                                         x.Category.Name.ToLower().Contains(condition.Keyword.ToLower()));
    }
}