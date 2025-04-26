using SothbeysKillerApi.Repository;
using SothbeysKillerApi.Services;

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

builder.Services.AddTransient<IAuctionRepository, DbAuctionRepository>();

/*
 * Transient
 * Scoped
 * Singleton
 */

var app = builder.Build();

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