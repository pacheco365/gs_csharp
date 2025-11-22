using EquilibriumModel;
using System.Collections.Generic; // Para o IEnumerable
using System.Threading.Tasks;

namespace EquilibriumApplication.Services
{
  public interface IUsuarioService
  {
    // CREATE
    Task<UsuarioResponseDto> CriarNovoUsuarioAsync(CriarUsuarioRequestDto request);

    // READ
    Task<UsuarioResponseDto?> GetUsuarioByIdAsync(int id);
    Task<IEnumerable<UsuarioResponseDto>> GetAllUsuariosAsync(); // <-- NOVO

    // UPDATE
    Task<bool> AtualizarUsuarioAsync(int id, AtualizarUsuarioRequestDto request); // <-- NOVO

    // DELETE
    Task<bool> DeletarUsuarioAsync(int id); // <-- NOVO
  }
}
