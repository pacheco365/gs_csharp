using EquilibriumModel;
using System;
using System.Collections.Generic; // Necessário para listas
using System.Threading.Tasks;

namespace EquilibriumApplication.Services
{
  public interface ITransacaoService
  {
    Task<CheckInResponseDto> RegistrarCheckInAsync(CheckInRequestDto request);

    Task<IEnumerable<CheckInResponseDto>> GetHistoricoUsuarioAsync(int usuarioId);

    Task<CheckInResponseDto?> GetCheckInByIdAsync(Guid idTransacao);

    Task<bool> AtualizarCheckInAsync(Guid idTransacao, int novoNivelHumor);

    Task<bool> DeletarCheckInAsync(Guid idTransacao);

    Task<CheckInResponseDto> ResgatarRecompensaAsync(ResgateRequestDto request);
  }
}
