using Microsoft.EntityFrameworkCore;
using NotaFacil.BillingService.Data;
using NotaFacil.BillingService.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar o banco de dados SQLite (ou o que você estiver usando)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=notaFacil.db"));

// Permitir CORS para o frontend Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.MapGet("/", () => "API Nota Fácil - Billing Service");

//  LISTAR TODAS AS NOTAS
app.MapGet("/invoices", async (AppDbContext db) =>
    await db.Invoices.Include(i => i.Items).ToListAsync());

// OBTER NOTA POR ID
app.MapGet("/invoices/{id}", async (AppDbContext db, int id) =>
{
    var invoice = await db.Invoices.Include(i => i.Items).FirstOrDefaultAsync(i => i.Id == id);
    return invoice is not null ? Results.Ok(invoice) : Results.NotFound();
});

// CRIAR NOVA NOTA
app.MapPost("/invoices", async (AppDbContext db, Invoice invoice) =>
{
    db.Invoices.Add(invoice);
    await db.SaveChangesAsync();
    return Results.Created($"/invoices/{invoice.Id}", invoice);
});

// ATUALIZAR NOTA EXISTENTE
app.MapPut("/invoices/{id}", async (AppDbContext db, int id, Invoice update) =>
{
    var invoice = await db.Invoices
        .Include(i => i.Items)
        .FirstOrDefaultAsync(i => i.Id == id);

    if (invoice is null)
        return Results.NotFound("Invoice não encontrada.");

    invoice.Numero = update.Numero;
    invoice.Descricao = update.Descricao;
    invoice.Valor = update.Valor;
    invoice.Date = update.Date;

    invoice.Items.Clear();
    foreach (var item in update.Items)
    {
        item.Id = 0; 
        invoice.Items.Add(item);
    }

    await db.SaveChangesAsync();

    var updatedInvoice = await db.Invoices
        .Include(i => i.Items)
        .FirstOrDefaultAsync(i => i.Id == id);

    return Results.Ok(updatedInvoice);
});

//  EXCLUIR NOTA
app.MapDelete("/invoices/{id}", async (AppDbContext db, int id) =>
{
    var invoice = await db.Invoices.FindAsync(id);
    if (invoice is null)
        return Results.NotFound();

    db.Invoices.Remove(invoice);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
