using SothbeysKillerApi.Repository;
using SothbeysKillerApi.Services;
using Dapper;

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
builder.Services.AddTransient<ILotRepository, DbLotRepository>();
builder.Services.AddTransient<IBidRepository, DbBidRepository>();
builder.Services.AddTransient<IUserRepository, DbUserRepository>();

builder.Services.AddScoped<System.Data.IDbConnection>(sp =>
    new Npgsql.NpgsqlConnection(
        sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var connection = scope.ServiceProvider.GetRequiredService<System.Data.IDbConnection>();
    connection.Open();

    connection.Execute(@"
        CREATE TABLE IF NOT EXISTS users (
            id UUID PRIMARY KEY,
            name TEXT NOT NULL,
            email TEXT NOT NULL UNIQUE,
            password TEXT NOT NULL
        );
        CREATE TABLE IF NOT EXISTS auctions (
            id UUID PRIMARY KEY,
            title TEXT NOT NULL,
            start TIMESTAMP NOT NULL,
            finish TIMESTAMP NOT NULL
        );
        CREATE TABLE IF NOT EXISTS lots (
            id UUID PRIMARY KEY,
            auctionid UUID NOT NULL REFERENCES auctions(id) ON DELETE CASCADE,
            title TEXT NOT NULL,
            description TEXT NOT NULL,
            startprice DECIMAL NOT NULL,
            pricestep DECIMAL NOT NULL
        );
        CREATE TABLE IF NOT EXISTS bids (
            id UUID PRIMARY KEY,
            lotid UUID NOT NULL REFERENCES lots(id) ON DELETE CASCADE,
            userid UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
            amount DECIMAL NOT NULL,
            created TIMESTAMP NOT NULL
        );
    ");
}

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