using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EquilibriumModel
{
  public class UsuariosModel
  {
    [Key]
    public int IdUsuario { get; set; }

    public int SaldoEQ { get; set; }

    public required string Nome { get; set; }

    public required string Email { get; set; }

    public required string Senha { get; set; }

    // Inicializamos a lista para evitar nulo
    public ICollection<TransacaoEQPoint> Transacoes { get; set; } = new List<TransacaoEQPoint>();
  }
}
