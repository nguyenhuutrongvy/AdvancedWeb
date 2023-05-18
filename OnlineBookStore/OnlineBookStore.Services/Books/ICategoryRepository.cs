using OnlineBookStore.Core.Constants;
using OnlineBookStore.Core.DTO;
using OnlineBookStore.Core.Entities;

namespace OnlineBookStore.Services.Books;

public interface ICategoryRepository
{
    Task<Category> GetCategoryByIdAsync(int authorId);

    Task<Category> GetCachedCategoryByIdAsync(int authorId);

    Task<IList<CategoryItem>> GetCategoriesAsync(string name = "", CancellationToken cancellationToken = default);

    Task<IList<Book>> GetBooksAsync<T>(BookQuery condition, Func<IQueryable<Book>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateAsync(Category author, CancellationToken cancellationToken = default);

    Task<bool> DeleteCategoryAsync(int authorId, CancellationToken cancellationToken = default);
}