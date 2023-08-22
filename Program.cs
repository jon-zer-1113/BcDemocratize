using BcDemocratize.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ajout de la connexion � la base de donn�es.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Error connecting to database");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// permet de visualiser les exceptions qui se produisent dans la base de donn�es pendant le d�veloppement.

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
// Ajout de ASP.NET Identity

var app = builder.Build();

// Configuration du pipeline de requ�tes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    // ajoute une page qui permet de g�rer les migrations de base de donn�es pendant le d�veloppement.
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // ajoute une gestion centralis�e des erreurs pour les erreurs non trait�es dans l'application.
    // La valeur HSTS par d�faut est de 30 jours.
    app.UseHsts();
    // ajoute la politique de s�curit� HTTP Strict Transport Security (HSTS) pour prot�ger l'application contre les attaques par injection de code malveillant (code injection).
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
J'ai modifi� l'option : "options.SignIn.RequireConfirmedAccount" en "false" pour �viter la confirmation par mail, car c'est un simple exercice
*/