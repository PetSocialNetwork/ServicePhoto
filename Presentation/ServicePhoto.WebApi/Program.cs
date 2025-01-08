using Microsoft.EntityFrameworkCore;
using ServicePhoto.DataEntityFramework;
using ServicePhoto.DataEntityFramework.Repositories;
using ServicePhoto.Domain.Interfaces;
using ServicePhoto.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped(typeof(IRepositoryEF<>), typeof(EFRepository<>));
builder.Services.AddScoped<IPetPhotoRepository, PetPhotoRepository>();
builder.Services.AddScoped<IPersonalPhotoRepository, PersonalPhotoRepository>();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
