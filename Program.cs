using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CustomInventoryApp.Data; 
using CustomInventoryApp.Models; 

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings__DefaultConnection"] 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try 
    {
        context.Database.EnsureCreated();
        Console.WriteLine("Successfully connected to the database and ensured schema exists.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"DATABASE CONNECTION FAILED: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();