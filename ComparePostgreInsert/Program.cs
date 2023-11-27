using GuidVSUuid7;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Host=localhost;Port=5432;Username=admin;Password=admin;Database=guid_uuid";
builder.Services.AddSingleton<INpgSqlConnectionDatabase>(new NpgSqlConnectionDatabase(connectionString));

var app = builder.Build();

var database = app.Services.GetService<INpgSqlConnectionDatabase>();
database.EnsureDatabase();
database.EnsureTable();

app.MapPost("/insert-guid/{row:int}", ([FromServices] INpgSqlConnectionDatabase database, int row) =>
{
    database.InsertDataGuid(row);
});

app.MapPost("/insert-uuid7/{row:int}", ([FromServices] INpgSqlConnectionDatabase database, int row) =>
{
    database.InsertDataGuidUUID7(row);
});

app.MapPost("/insert-long/{row:int}", ([FromServices] INpgSqlConnectionDatabase database, int row) =>
{
    database.InsertDataGuidLong(row);
});

app.MapPost("/insert-newid/{row:int}", ([FromServices] INpgSqlConnectionDatabase database, int row) =>
{
    database.InsertDataNewId(row);
});

app.Run();