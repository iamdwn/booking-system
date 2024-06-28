using Microsoft.EntityFrameworkCore;
using Quartz;
using Repository.Models;
using Repository.Repositories.Impl;
using Repository.Repositories.Interfaces;
using Service;
using Service.Services.Impl;
using Service.Services.Interfaces;
using Service.Services.Quartzs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<FuminiHotelManagementContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Register SignalR
builder.Services.AddSignalR();
builder.Services.AddTransient<BookingHub>();

// Enable session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});

//Register repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBookingReservationRepository, BookingReservationRepository>();
builder.Services.AddScoped<IRoomInformationRepository, RoomInformationRepository>();
builder.Services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();

//Register service
builder.Services.AddSingleton<IEmailQueue, EmailQueue>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IRoomService, RoomService>();

//Register quartz
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Configure SendEmailJob
    var emailJobKey = new JobKey("SendEmailJob");
    q.AddJob<SendEmailJob>(opts => opts.WithIdentity(emailJobKey));

    var emailCronSchedule = builder.Configuration.GetSection("CronJobs:SendEmailJob")?.Value;
    if (string.IsNullOrWhiteSpace(emailCronSchedule))
    {
        throw new ArgumentException("The cron schedule for SendEmailJob is not configured properly.");
    }

    q.AddTrigger(opts => opts
        .ForJob(emailJobKey)
        .WithIdentity("SendEmailJob-trigger")
        .WithCronSchedule(emailCronSchedule));

    // Configure UpdateRoomStatusJob
    var statusJobKey = new JobKey("UpdateRoomStatusJob");
    q.AddJob<UpdateRoomStatusJob>(opts => opts.WithIdentity(statusJobKey));

    var statusCronSchedule = builder.Configuration.GetSection("CronJobs:UpdateRoomStatusJob")?.Value;
    if (string.IsNullOrWhiteSpace(statusCronSchedule))
    {
        throw new ArgumentException("The cron schedule for UpdateRoomStatusJob is not configured properly.");
    }

    q.AddTrigger(opts => opts
        .ForJob(statusJobKey)
        .WithIdentity("UpdateRoomStatusJob-trigger")
        .WithCronSchedule(statusCronSchedule));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapHub<BookingHub>("/bookingHub");

app.Run();
