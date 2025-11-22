using Microsoft.EntityFrameworkCore;
using EquilibriumData;
using EquilibriumBusiness;
using EquilibriumApplication.Services;
using Oracle.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Banco de Dados (Oracle)

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AplicattionDBContext>(options =>
    options.UseOracle(connectionString, b =>
        b.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

// 3. Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
