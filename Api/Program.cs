using Api.Models;
using Api.Repository;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ConfigureServices(builder.Services);

            var app = builder.Build();

            InitializeDbData(app.Services.GetService<UserDBContext>());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDBContext>(options => options.UseInMemoryDatabase("Users")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning)).EnableDetailedErrors(), ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
        }
        private static void InitializeDbData(UserDBContext userDBContext)
        {
            var numberOfUsers = Helpers.Helpers.GenerateRandomNumber(50);

            var universityNameCounter = 1;
            var universityName = Helpers.Helpers.GenerateRandomString();
            var university = new University()
            {
                Id = universityNameCounter,
                Name = universityName,
                Score = Helpers.Helpers.GenerateRandomNumber(100, 50)
            };

            userDBContext.Universities.Add(university);

            for (int i = 1; i <= numberOfUsers; i++)
            {
                var user = new User
                {
                    Id = i,
                    NumberOfPublications = Helpers.Helpers.GenerateRandomNumber(10),
                    UserName = Helpers.Helpers.GenerateRandomString(),
                    UniversityName = universityName
                };

                userDBContext.Users.Add(user);

                if (universityNameCounter > 5)
                {
                    universityName = Helpers.Helpers.GenerateRandomString();
                    universityNameCounter = 0;

                    university = new University()
                    {
                        Id = universityNameCounter,
                        Name = universityName,
                        Score = Helpers.Helpers.GenerateRandomNumber(100)
                    };

                    userDBContext.Universities.Add(university);
                }
                else
                {
                    universityNameCounter++;
                }
            }

            userDBContext.SaveChanges();
        }
    }
}