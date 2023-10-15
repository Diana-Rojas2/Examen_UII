using Examen_UII.Hubs;
using Examen_UII.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AguaDBContext>(opciones =>{
    opciones.UseSqlServer(
        builder.Configuration.GetConnectionString("AguaDB"));
});

builder.Services.AddSignalR();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
).AddCookie(opciones =>{
    opciones.LoginPath = new PathString("/Usuarios/Login");
    opciones.AccessDeniedPath = new PathString("/Usuarios/NoPermitido");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.MapHub<MensajeHub>("/WebSocketServer");
app.MapHub<MapHub>("/mapHub");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}");

app.Run();
