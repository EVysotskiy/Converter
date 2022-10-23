using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Database;
using Server.HostedServices;
using Server.Options;
using Server.Repositories;
using Server.Services;
using Telegram.Bot;
using TelegramBot.Command;
using TelegramBot.Configuration;
using TelegramBot.Handler;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddControllers();

builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x =>
    new TelegramBotClient(x.GetService<IOptions<TelegramOptions>>()?.Value.Token ?? string.Empty));

//Options
{
    builder.Services.Configure<TelegramOptions>(configuration.GetSection(TelegramOptions.Position));
    builder.Services.Configure<IronOptions>(configuration.GetSection(IronOptions.Position));
}

// Services
{
    builder.Services.AddScoped<IUserServices, UserServices>();
    builder.Services.AddScoped<IConverterService, ConvertService>();
    builder.Services.AddScoped<IDocumentService, DocumentService>();
    builder.Services.Decorate<IUserServices, CachedUserServices>();
    builder.Services.AddHostedService<TelegramBotWorker>();
}

//Command
{
    builder.Services.AddTransient<ICommandFactory, CommandFactory>();
    builder.Services.AddScoped<IUpdatesHandler, UpdatesHandler>();
}

//Repository
{
    builder.Services.AddScoped<UserRepository>();
    builder.Services.AddScoped<DocumentRepository>();
}

builder.Services.AddDbContextFactory<AppDbContext>(ConfigurePostgresConnection);
builder.Services.AddDbContext<AppDbContext>(ConfigurePostgresConnection);
builder.Services.AddMemoryCache();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

void ConfigurePostgresConnection(DbContextOptionsBuilder options)
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlContext"));
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
app.Run();