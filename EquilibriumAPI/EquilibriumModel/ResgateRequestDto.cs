using System.ComponentModel.DataAnnotations;

namespace EquilibriumModel
{
  public class ResgateRequestDto
  {
    [Required]
    public int UsuarioId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O valor do resgate deve ser positivo.")]
    public int ValorGasto { get; set; }

    [Required]
    public required string DescricaoRecompensa { get; set; }
  }
}
