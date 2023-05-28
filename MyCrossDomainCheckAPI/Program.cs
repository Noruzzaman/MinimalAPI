using MyCrossDomainCheckAPI.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddCors();




builder.Services.AddDbContext<FurnitureDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FurnitureDbConnection"));
});
//builder.Services.AddMvc();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// get endpoint
app.MapGet("/furnitures", async (FurnitureDbContext FurnitureDb) =>
{
    var furnitures = await FurnitureDb.Furnitures.ToListAsync();
    return Results.Ok(furnitures);
});
// post endpoint
app.MapPost("/furnitures/create", async (Furniture furnitures, FurnitureDbContext FurnitureDb) =>
{
    FurnitureDb.Furnitures.Add(furnitures);
    await FurnitureDb.SaveChangesAsync();
    return Results.Ok();
});

// update endpoint
app.MapPut("/furnitures/update/{id}", async (int id, Furniture FurnituresToUpdate, FurnitureDbContext FurnitureDb) =>
{
    var furnitures = await FurnitureDb.Furnitures.FindAsync(id);

    if (furnitures is null)
    {
        return Results.NotFound();
    }
    else
    {



        furnitures.Brand = FurnituresToUpdate.Brand;
        furnitures.Cost = FurnituresToUpdate.Cost;
        furnitures.FurnitureName = FurnituresToUpdate.FurnitureName;
        furnitures.FurnitureType = FurnituresToUpdate.FurnitureType;


        await FurnitureDb.SaveChangesAsync();

        return Results.Ok();
    }
});

// delete endpoint.
app.MapDelete("/furnitures/delete/{id}", async (int id, FurnitureDbContext FurnitureDb) =>
{
    var dbFurniturs = await FurnitureDb.Furnitures.FindAsync(id);
    if (dbFurniturs == null)
    {
        return Results.NoContent();
    }
    FurnitureDb.Furnitures.Remove(dbFurniturs);
    await FurnitureDb.SaveChangesAsync();
    return Results.Ok();
});


app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
            );


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
