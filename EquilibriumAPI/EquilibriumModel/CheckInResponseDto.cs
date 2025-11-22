using System;

namespace EquilibriumModel
{
  public class CheckInResponseDto
  {
    public Guid IdTransacao { get; set; }
    public int UsuarioId { get; set; }
    public int PontosGanhos { get; set; }
    public int NovoSaldoTotal { get; set; }
    public DateTime DataCheckIn { get; set; }

    public required string Descricao { get; set; }
  }
}
