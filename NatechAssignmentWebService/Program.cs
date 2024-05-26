using Hangfire;
using Microsoft.EntityFrameworkCore;
using NatechAssignmentBusinessObject;
using NatechAssignmentCommon.Configuration;
using NatechAssignmentCommon.Interface;
using NatechAssignmentDataLayer;

namespace NatechAssignmentWebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(configure =>
            {
                configure.AddDebug();
            });

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LocateDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Hangfire services.
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add the processing server as IHostedService
            builder.Services.AddHangfireServer();

            builder.Services.AddTransient<INatechBusinessObject, NatechBusinessObject>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHangfireDashboard();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            RecurringJob.AddOrUpdate<NatechBackgroundJob>(
                Jobs.ProcessJob,
                x => x.Process(),
                Cron.Minutely());

            app.Run();
        }
    }
}
