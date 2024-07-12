using System.Net.Mail;
using Hangfire;
using Hangfire.PostgreSql;
using HRApp.DataAccess;
using HRApp.Utilits;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services);

var app = builder.Build();

Configure(app);

var emailSender = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetRequiredService<IEmailSender>();
RecurringJob.AddOrUpdate("DailyEmailJob", () => emailSender.SendDailyEmail(), Cron.Daily);

app.Run();

void RegisterServices(IServiceCollection services)
{
    services.AddControllers();
    
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddDbContext<HRAppDb>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(HRAppDb)));
    });

    services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c =>
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("HangfireConnection"))));
    services.AddHangfireServer();

    services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    services.AddScoped<IOrderRepository, OrderRepository>();

    services.AddScoped<IEmailSender, EmailSender>();
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HRAppDb>();
        db.Database.EnsureCreated();
    }

    app.UseHttpsRedirection();

    app.UseHangfireDashboard();

    app.UseAuthorization();

    app.MapControllers();
}