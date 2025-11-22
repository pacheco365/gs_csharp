using System.ComponentModel.DataAnnotations;

namespace EquilibriumModel
{
  public class CheckInRequestDto
  {
    [Required]
    public int UsuarioId { get; set; }

    [Range(1, 5)] // Ex: 1=Péssimo, 5=Ótimo
    public int NivelHumor { get; set; }
  }
}
