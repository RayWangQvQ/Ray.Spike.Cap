using DotNetCore.CAP.Serialization;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Ray.Spike.Cap
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

            builder.Services.AddSingleton<ISerializer, YourSerializer>();
            builder.Services.AddCap(x =>
            {
                //x.UseInMemoryStorage();

                x.UseSqlite(cfg =>
                {
                    cfg.ConnectionString = "Data Source=./cap-event.db";
                });

                x.UseInMemoryMessageQueue();

                x.UseDashboard();
            })
            .AddSubscribeFilter<MyCapFilter>()
            ;

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
