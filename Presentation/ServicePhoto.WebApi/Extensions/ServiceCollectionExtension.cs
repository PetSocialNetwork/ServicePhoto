using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using ServicePhoto.DataEntityFramework.Repositories;
using ServicePhoto.DataEntityFramework;
using ServicePhoto.Domain.Interfaces;
using ServicePhoto.Domain.Services;
using ServicePhoto.WebApi.Filters;
using ServicePhoto.FileStorage.Configurations;
using ServicePhoto.FileStorage.Services;

namespace ServicePhoto.WebApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);
            services.AddApplicationComponents(configuration);
        }

        public static void ConfigureSettings
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileSettings>
                (configuration.GetRequiredSection("FileSettings"));
        }

        public static void ConfigureMiddleware(this WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "wwwroot", "Images")),
                RequestPath = "/images"
            });

            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
        }

        private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddControllers(options =>
            {
                options.Filters.Add<CentralizedExceptionHandlingFilter>();
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoService", Version = "v1" });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddApplicationComponents(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories(configuration);
            services.AddDomainServices();
        }

        private static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
              options.UseNpgsql(configuration.GetConnectionString("Postgres")));

            services.AddScoped(typeof(IRepositoryEF<>), typeof(EFRepository<>));
            services.AddScoped<IPetPhotoRepository, PetPhotoRepository>();
            services.AddScoped<IPersonalPhotoRepository, PersonalPhotoRepository>();
        }

        private static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<PetPhotoService>();
            services.AddScoped<PersonalPhotoService>();
        }
    }
}
