using DotNetCore.CAP;using DotNetCore.CAP.Serialization;using Microsoft.Extensions.Hosting;using Ray.Spike.Cap.Common;using Savorboard.CAP.InMemoryMessageQueue;using Serilog.Debugging;using Serilog;using StackExchange.Redis;using System.Net;namespace Ray.Spike.Cap{    public class Program    {        public static void Main(string[] args)        {            var builder = WebApplication.CreateBuilder(args);

            builder.Host.ConfigureLogging((hostBuilderContext, loggingBuilder) =>
            {
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("cap.log")
                .CreateLogger();
            })
                .UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers();            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle            builder.Services.AddEndpointsApiExplorer();            builder.Services.AddSwaggerGen();            builder.Services.AddSingleton<ISerializer, EncryptSerializer>();            //builder.Services.AddSingleton<ISerializer, BodyEncryptSerializer>();            builder.Services.AddCap(x =>            {                //x.UseInMemoryStorage();                x.UseSqlite(cfg =>                {                    cfg.ConnectionString = "Data Source=./cap-event.db";                });                //x.UseSqlServer(opt => {                //    //SqlServerOptions                //});                //x.UseInMemoryMessageQueue();                x.UseRedis(redisOptions => {
                    redisOptions.Configuration = ConfigurationOptions.Parse(builder.Configuration["ConnectionStrings:Redis"]);
                });                x.UseDashboard();            })            .AddSubscribeFilter<MyCapFilter>()            ;            builder.Services.AddDataProtection();            var app = builder.Build();            // Configure the HTTP request pipeline.            if (app.Environment.IsDevelopment())            {                app.UseSwagger();                app.UseSwaggerUI();            }            app.UseAuthorization();            app.MapControllers();            app.Run();        }    }}