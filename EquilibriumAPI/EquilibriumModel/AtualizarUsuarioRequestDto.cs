using System.ComponentModel.DataAnnotations;

namespace EquilibriumModel
{
  public class AtualizarUsuarioRequestDto
  {
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public required string Nome { get; set; } // Adicionado 'required'

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress]
    public required string Email { get; set; } // Adicionado 'required'
  }
}
