using Microsoft.EntityFrameworkCore;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Context;
using TiendaCosmeticos.Infrastructure.Repositories;
using TiendaCosmeticos.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<MarcaRepository>();
builder.Services.AddTransient<ProductoRepository>();

builder.Services.AddTransient<GenericRepository<Cliente>>();
builder.Services.AddTransient<GenericRepository<Venta>>();

builder.Services.AddTransient<UnitOfWork>();

builder.Services.AddTransient<MarcaService>();
builder.Services.AddTransient<ProductoService>();
builder.Services.AddTransient<ClienteService>();
builder.Services.AddTransient<VentaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
