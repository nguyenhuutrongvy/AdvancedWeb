using OnlineBookStore.Data.Contexts;
using OnlineBookStore.Data.Seeders;

var context = new BookDbContext();
var seeder = new DataSeeder(context);

seeder.Initialize();