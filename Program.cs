using Microsoft.EntityFrameworkCore;
using Practica.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure SQLite for demo
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=practica.db"));

var app = builder.Build();

// Ensure DB created and seed sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    if (!db.Personas.Any())
    {
        db.Personas.AddRange(
            new Practica.Models.Persona { Nombre = "María", Edad = 22 },
            new Practica.Models.Persona { Nombre = "Luis", Edad = 30 },
            new Practica.Models.Persona { Nombre = "Ana", Edad = 27 }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Personas}/{action=Index}/{id?}"
);

app.Run();
