using SothbeysKillerApi.Repository;
using SothbeysKillerApi.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Contexts;
using SothbeysKillerApi.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAuctionService, AuctionService>();
builder.Services.AddTransient<IBidService, BidService>();
builder.Services.AddTransient<ILotService, LotService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IAuctionRepository, EfAuctionRepository>();
builder.Services.AddTransient<ILotRepository, EfLotRepository>();
builder.Services.AddTransient<IBidRepository, EfBidRepository>();
builder.Services.AddTransient<IUserRepository, EfUserRepository>();

builder.Services.AddExceptionHandler<UserValidationExceptionHandler>();
builder.Services.AddExceptionHandler<BidValidationExceptionHandler>();
builder.Services.AddExceptionHandler<LotValidationExceptionHandler>();
builder.Services.AddExceptionHandler<AuctionValidationExceptionHandler>();
builder.Services.AddExceptionHandler<ServerExceptionsHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();