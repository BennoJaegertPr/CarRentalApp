using CarRentalApp.Db;
using CarRentalApp.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDatabase"));

var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // Check if there are any existing users
    if (!dbContext.Customer.Any() && !dbContext.Car.Any())
    {
        // Seed the database with initial customers
        dbContext.Customer.AddRange(
            new Customer { Firstname = "John", Lastname = "Doe", BirthDate = DateTime.ParseExact("18.09.1998", "dd.MM.yyyy", CultureInfo.InvariantCulture), Phonenumber = "+4366012345678"},
            new Customer { Firstname = "Jane", Lastname = "Doe", BirthDate = DateTime.ParseExact("18.09.1998", "dd.MM.yyyy", CultureInfo.InvariantCulture), Phonenumber = "+4366098765432" }
        );

        // Seed the database with initial cars
        dbContext.Car.AddRange(
            new Car { Brand="BMW", Color="Red", Km="34526" },
            new Car { Brand = "Audi", Color = "Black", Km = "28376", isAvailable=false }
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
