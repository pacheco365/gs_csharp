using System;
using System.ComponentModel.DataAnnotations;

namespace EquilibriumModel
{
  public class TransacaoEQPoint
  {
    [Key]
    public Guid IdTransacao { get; set; }

    public int Valor { get; set; }


    public required string Descricao { get; set; }

    public DateTime DataTransacao { get; set; }
    public int UsuarioId { get; set; }

    // ADICIONADO '?': Indica que o objeto Usuario pode ser nulo (opcional no carregamento)
    public UsuariosModel? Usuario { get; set; }
  }
}
