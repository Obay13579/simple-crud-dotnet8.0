using Microsoft.EntityFrameworkCore;
using MigrationTestApp.Data;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var mysqlConnString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mysqlConnString, ServerVersion.AutoDetect(mysqlConnString)));

var blobConnString = builder.Configuration.GetConnectionString("BlobStorageConnection");
builder.Services.AddSingleton(new BlobServiceClient(blobConnString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Orders}/{action=Index}/{id?}");

app.Run();