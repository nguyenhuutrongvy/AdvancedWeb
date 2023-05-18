namespace OnlineBookStore.Services.Books;

public interface IAdminRepository
{
    Task<bool> Check(string account, string password);
}