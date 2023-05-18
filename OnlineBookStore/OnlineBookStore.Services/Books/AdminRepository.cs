using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineBookStore.Core.Entities;
using OnlineBookStore.Data.Contexts;
using System.Web;

namespace OnlineBookStore.Services.Books;

public class AdminRepository : IAdminRepository
{
    private readonly BookDbContext _context;

    public AdminRepository(BookDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
    }

    public async Task<bool> Check(string account, string password)
    {
        //var result = (await _context.Set<Admin>().FirstOrDefaultAsync(a => HttpUtility.UrlDecode(account).Equals(a.Email) && HttpUtility.UrlDecode(password).Equals(a.Password)));
        var result = (await _context.Set<Admin>().FirstOrDefaultAsync(a => a.Email.Equals(account) && a.Password.Equals(password)));
        return result == null ? false : true;
    }
}