using Microsoft.EntityFrameworkCore;
using NotaFacil.InventoryService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=inventory.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

app.MapGet("/products/{id}", async (int id, AppDbContext db) =>
    await db.Products.FindAsync(id)
        is Product p ? Results.Ok(p) : Results.NotFound());

app.MapPost("/products", async (AppDbContext db, Product product) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

// PUT /products/{id} -> Atualizar um produto
app.MapPut("/products/{id}", async (int id, AppDbContext db, Product input) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null)
        return Results.NotFound();

    product.Name = input.Name;
    product.Price = input.Price;

    await db.SaveChangesAsync();
    return Results.Ok(product);
});

// DELETE /products/{id} -> Excluir um produto
app.MapDelete("/products/{id}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null)
        return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.NoContent();
});



app.Run();
