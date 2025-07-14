using DeferredTaskAPI.Application.Services;
using DeferredTaskAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using TaskSchedulerProject.BackgroundServices;
using TaskSchedulerProject.DataAccess.Context;
using TaskSchedulerProject.Interfaces;

namespace TaskSchedulerProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Enable console logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IScheduledTaskRepository, ScheduledTaskRepository>();
            builder.Services.AddScoped<IScheduledTaskService, ScheduledTaskService>();

            builder.Services.AddHostedService<ScheduledTaskExecute>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}
