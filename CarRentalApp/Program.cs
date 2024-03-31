using CarRentalApp.Db;
using CarRentalApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



// Load configuration from appsettings.json
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(logger);

// Retrieve connection string
string? connectionString = config.GetConnectionString("DefaultConnection");

// Retrieve value indicating whether to use in-memory database
bool useInMemoryDatabase = config.GetValue<bool>("UseInMemoryDatabase");

if (useInMemoryDatabase){
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("InMemoryDatabase"));
}
else {
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}


var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // Check if there are any existing users
    if (!dbContext.Customers.Any() && !dbContext.Cars.Any() && useInMemoryDatabase){
        // Seed the database with initial customers
        dbContext.Customers.AddRange(
            new Customer { Id=1 ,Firstname = "John", Lastname = "Doe", BirthDate = DateTime.ParseExact("18.09.1998", "dd.MM.yyyy", CultureInfo.InvariantCulture), Phonenumber = "+4366012345678"},
            new Customer { Id=2 ,Firstname = "Jane", Lastname = "Doe", BirthDate = DateTime.ParseExact("07.10.2004", "dd.MM.yyyy", CultureInfo.InvariantCulture), Phonenumber = "+4366098765432" }
        );

        // Seed the database with initial cars
        dbContext.Cars.AddRange(
            new Car {Id=1 ,Brand="BMW", Color="Red", Km=34526, PricePerDay=75 },
            new Car {Id=2 ,Brand = "Audi", Color = "Black", Km = 28376, PricePerDay = 62, isAvailable=false }
        );

        // Save changes to the database
        dbContext.SaveChanges();

        // Retrieve the customers and cars from the database
        var johnDoe = dbContext.Customers.Single(c => c.Firstname == "John" && c.Lastname == "Doe");
        var janeDoe = dbContext.Customers.Single(c => c.Firstname == "Jane" && c.Lastname == "Doe");

        var bmw = dbContext.Cars.Single(c => c.Brand == "BMW" && c.Color == "Red");
        var audi = dbContext.Cars.Single(c => c.Brand == "Audi" && c.Color == "Black");

        // Seed the database with initial cars
        dbContext.Rentals.AddRange(
            new Rental { Customer = johnDoe, Car= bmw, RentedPeriodInDays = 14 },
            new Rental { Customer = janeDoe, Car = audi, RentedPeriodInDays = 4 }
        );

        // Save changes to the database
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
