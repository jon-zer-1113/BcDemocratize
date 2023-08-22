using BcDemocratize.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ajout de la connexion à la base de données.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Error connecting to database");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// permet de visualiser les exceptions qui se produisent dans la base de données pendant le développement.

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
// Ajout de ASP.NET Identity

var app = builder.Build();

// Configuration du pipeline de requêtes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    // ajoute une page qui permet de gérer les migrations de base de données pendant le développement.
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // ajoute une gestion centralisée des erreurs pour les erreurs non traitées dans l'application.
    // La valeur HSTS par défaut est de 30 jours.
    app.UseHsts();
    // ajoute la politique de sécurité HTTP Strict Transport Security (HSTS) pour protéger l'application contre les attaques par injection de code malveillant (code injection).
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

/*
J'ai modifié l'option : "options.SignIn.RequireConfirmedAccount" en "false" pour éviter la confirmation par mail, car c'est un simple exercice
*/