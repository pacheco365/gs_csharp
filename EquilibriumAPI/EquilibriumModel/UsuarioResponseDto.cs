namespace EquilibriumModel
{
  public class UsuarioResponseDto
  {
    public int IdUsuario { get; set; }

    public required string Nome { get; set; }

    public required string Email { get; set; }

    public int SaldoEQ { get; set; }
  }
}
