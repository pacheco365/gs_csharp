using System.ComponentModel.DataAnnotations;

namespace EquilibriumModel
{
  public class AtualizarCheckInRequestDto
  {
    [Required]
    [Range(1, 5, ErrorMessage = "O nível de humor deve ser entre 1 e 5.")]
    public int NovoNivelHumor { get; set; }
  }
}
