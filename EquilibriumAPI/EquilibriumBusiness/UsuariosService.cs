using Microsoft.EntityFrameworkCore;
using EquilibriumData;
using EquilibriumModel;
using EquilibriumApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquilibriumBusiness
{
  public class UsuarioService : IUsuarioService
  {
    private readonly AplicattionDBContext _context;

    public UsuarioService(AplicattionDBContext context)
    {
      _context = context;
    }

    public async Task<UsuarioResponseDto> CriarNovoUsuarioAsync(CriarUsuarioRequestDto request)
    {
      var emailJaExiste = await _context.UsuariosGS
                                        .AnyAsync(u => u.Email == request.Email);
      if (emailJaExiste)
      {
        throw new Exception("Este e-mail já está em uso.");
      }

      var novoUsuario = new UsuariosModel
      {
        Nome = request.Nome,
        Email = request.Email,
        Senha = request.Senha,
        SaldoEQ = 0
      };

      _context.UsuariosGS.Add(novoUsuario);
      await _context.SaveChangesAsync();

      return MapearParaResponseDto(novoUsuario);
    }

    public async Task<UsuarioResponseDto?> GetUsuarioByIdAsync(int id)
    {
      var usuario = await _context.UsuariosGS.FindAsync(id);
      return usuario == null ? null : MapearParaResponseDto(usuario);
    }

    public async Task<IEnumerable<UsuarioResponseDto>> GetAllUsuariosAsync()
    {
      var usuarios = await _context.UsuariosGS.ToListAsync();


      return usuarios.Select(usuario => MapearParaResponseDto(usuario));
    }

    public async Task<bool> AtualizarUsuarioAsync(int id, AtualizarUsuarioRequestDto request)
    {
      var usuario = await _context.UsuariosGS.FindAsync(id);
      if (usuario == null)
      {
        return false; // Usuário não encontrado
      }

      var emailConflitante = await _context.UsuariosGS
          .AnyAsync(u => u.Email == request.Email && u.IdUsuario != id);

      if (emailConflitante)
      {
        throw new Exception("Este e-mail já está em uso por outra conta.");
      }

      // Atualiza os dados
      usuario.Nome = request.Nome;
      usuario.Email = request.Email;

      _context.UsuariosGS.Update(usuario);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> DeletarUsuarioAsync(int id)
    {
      var usuario = await _context.UsuariosGS.FindAsync(id);
      if (usuario == null)
      {
        return false; // Usuário não encontrado
      }

      _context.UsuariosGS.Remove(usuario);
      await _context.SaveChangesAsync();
      return true;
    }

    private UsuarioResponseDto MapearParaResponseDto(UsuariosModel usuario)
    {
      return new UsuarioResponseDto
      {
        IdUsuario = usuario.IdUsuario,
        Nome = usuario.Nome,
        Email = usuario.Email,
        SaldoEQ = usuario.SaldoEQ
      };
    }
  }
}
