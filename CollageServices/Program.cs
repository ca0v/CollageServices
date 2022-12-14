using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ImageRipper.PhotoContext>();

builder.Services.AddControllers();

builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapRazorPages();

// serve static files from wwwroot
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider("/home/ca0v/code/ca0v/svelte-lab/dist"),
    RequestPath = new PathString("/svelte-lab"),
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider("/home/ca0v/code/corey-alix/collage-builder/dist"),
    RequestPath = new PathString("/collage-builder"),
});

// read "AllowedOrigins" from appsettings.json
var origins = builder.Configuration.GetValue<string>("AllowedOrigins")?.Split(';');
if (origins == null)
{
    throw new Exception("Cors whitelist not found in appsettings.json");
}

app.UseCors(policy => policy.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("*"));

app.MapControllers();

app.Run();

