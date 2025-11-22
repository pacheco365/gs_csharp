using Microsoft.EntityFrameworkCore;
using EquilibriumModel;
namespace EquilibriumData
{
  public class AplicattionDBContext(DbContextOptions<AplicattionDBContext> options) : DbContext(options)
  {
    public DbSet<TransacaoEQPoint> TransacaoGS { get; set; }

    public DbSet<UsuariosModel> UsuariosGS { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configura a relação (Lado 1: Usuário)
      modelBuilder.Entity<UsuariosModel>()
          .HasMany(u => u.Transacoes) // Um Usuário tem muitas Transações
          .WithOne(t => t.Usuario)    // Uma Transação tem um Usuário
          .HasForeignKey(t => t.UsuarioId); // A chave é 'UsuarioId'

      // Garante que o Email seja único
      modelBuilder.Entity<UsuariosModel>()
          .HasIndex(u => u.Email)
          .IsUnique();
    }
  }
}
