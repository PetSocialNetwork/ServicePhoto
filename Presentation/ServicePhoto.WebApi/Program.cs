using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using ServicePhoto.DataEntityFramework;
using ServicePhoto.DataEntityFramework.Repositories;
using ServicePhoto.Domain.Interfaces;
using ServicePhoto.Domain.Services;
using ServicePhoto.WebApi.Services.Implementations;
using ServicePhoto.WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoService", Version = "v1" });
    //options.UseAllOfToExtendReferenceSchemas();
    //string pathToXmlDocs = Path.Combine(AppContext.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".xml");
    //options.IncludeXmlComments(pathToXmlDocs, true);
});

builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped(typeof(IRepositoryEF<>), typeof(EFRepository<>));
builder.Services.AddScoped<IPetPhotoRepository, PetPhotoRepository>();
builder.Services.AddScoped<IPersonalPhotoRepository, PersonalPhotoRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<PetPhotoService>();
builder.Services.AddScoped<PersonalPhotoService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "Images")),
    RequestPath = "/images"
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
