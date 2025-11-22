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
  public class TransacaoService : ITransacaoService
  {
    private readonly AplicattionDBContext _context;
    private const int PONTOS_POR_CHECKIN = 10;

    public TransacaoService(AplicattionDBContext context)
    {
      _context = context;
    }

    // --- CREATE (Check-In) ---
    public async Task<CheckInResponseDto> RegistrarCheckInAsync(CheckInRequestDto request)
    {
      using var dbTransaction = await _context.Database.BeginTransactionAsync();
      try
      {
        var usuario = await _context.UsuariosGS.FindAsync(request.UsuarioId);
        if (usuario == null) throw new Exception("Usuário não encontrado.");

        var novaTransacao = new TransacaoEQPoint
        {
          IdTransacao = Guid.NewGuid(),
          UsuarioId = usuario.IdUsuario,
          Valor = PONTOS_POR_CHECKIN,
          Descricao = $"Check-in de humor (Nível: {request.NivelHumor})",
          DataTransacao = DateTime.UtcNow,
        };

        usuario.SaldoEQ += PONTOS_POR_CHECKIN;

        _context.UsuariosGS.Update(usuario);
        _context.TransacaoGS.Add(novaTransacao);

        await _context.SaveChangesAsync();
        await dbTransaction.CommitAsync();

        return MapearDto(novaTransacao, usuario.SaldoEQ);
      }
      catch
      {
        await dbTransaction.RollbackAsync();
        throw;
      }
    }

    // --- READ (Listar Todos do Usuário) ---
    public async Task<IEnumerable<CheckInResponseDto>> GetHistoricoUsuarioAsync(int usuarioId)
    {
      // Busca todas as transações daquele usuário
      var transacoes = await _context.TransacaoGS
                                     .Where(t => t.UsuarioId == usuarioId)
                                     .OrderByDescending(t => t.DataTransacao) // Mais recentes primeiro
                                     .ToListAsync();

      // Como não carregamos o Usuário junto, não temos o "Saldo Atual" exato na hora da transação
      // Vamos retornar 0 ou o saldo atual do usuário.
      // Para simplificar, vamos buscar o saldo atual do usuário uma vez.
      var usuario = await _context.UsuariosGS.FindAsync(usuarioId);
      int saldoAtual = usuario?.SaldoEQ ?? 0;

      return transacoes.Select(t => MapearDto(t, saldoAtual));
    }

    // --- READ (Por ID) ---
    public async Task<CheckInResponseDto?> GetCheckInByIdAsync(Guid idTransacao)
    {
      var transacao = await _context.TransacaoGS.FindAsync(idTransacao);
      if (transacao == null) return null;

      var usuario = await _context.UsuariosGS.FindAsync(transacao.UsuarioId);

      return MapearDto(transacao, usuario?.SaldoEQ ?? 0);
    }

    // --- UPDATE (Mudar o humor) ---
    public async Task<bool> AtualizarCheckInAsync(Guid idTransacao, int novoNivelHumor)
    {
      var transacao = await _context.TransacaoGS.FindAsync(idTransacao);
      if (transacao == null) return false;

      // Atualiza apenas a descrição, pois os pontos (10) continuam os mesmos
      transacao.Descricao = $"Check-in de humor (Nível: {novoNivelHumor}) - Editado";

      _context.TransacaoGS.Update(transacao);
      await _context.SaveChangesAsync();
      return true;
    }

    // --- DELETE (Remover e Estornar Pontos) ---
    public async Task<bool> DeletarCheckInAsync(Guid idTransacao)
    {
      using var dbTransaction = await _context.Database.BeginTransactionAsync();
      try
      {
        var transacao = await _context.TransacaoGS.FindAsync(idTransacao);
        if (transacao == null) return false;

        var usuario = await _context.UsuariosGS.FindAsync(transacao.UsuarioId);
        if (usuario != null)
        {
          // ESTORNO: Se a transação deu +10 pontos, agora tiramos 10.
          // Se a transação gastou -50, agora devolvemos 50 (menos com menos dá mais).
          usuario.SaldoEQ -= transacao.Valor;
          _context.UsuariosGS.Update(usuario);
        }

        _context.TransacaoGS.Remove(transacao);

        await _context.SaveChangesAsync();
        await dbTransaction.CommitAsync();
        return true;
      }
      catch
      {
        await dbTransaction.RollbackAsync();
        throw;
      }
    }

    // --- RESGATE (Mantido) ---
    public async Task<CheckInResponseDto> ResgatarRecompensaAsync(ResgateRequestDto request)
    {
      using var dbTransaction = await _context.Database.BeginTransactionAsync();
      try
      {
        var usuario = await _context.UsuariosGS.FindAsync(request.UsuarioId);
        if (usuario == null) throw new Exception("Usuário não encontrado.");

        if (usuario.SaldoEQ < request.ValorGasto)
          throw new Exception($"Saldo insuficiente. Saldo: {usuario.SaldoEQ}");

        var novaTransacao = new TransacaoEQPoint
        {
          IdTransacao = Guid.NewGuid(),
          UsuarioId = usuario.IdUsuario,
          Valor = -request.ValorGasto, // Negativo
          Descricao = $"Resgate: {request.DescricaoRecompensa}",
          DataTransacao = DateTime.UtcNow
        };

        usuario.SaldoEQ -= request.ValorGasto;

        _context.UsuariosGS.Update(usuario);
        _context.TransacaoGS.Add(novaTransacao);

        await _context.SaveChangesAsync();
        await dbTransaction.CommitAsync();

        return MapearDto(novaTransacao, usuario.SaldoEQ);
      }
      catch
      {
        await dbTransaction.RollbackAsync();
        throw;
      }
    }

    // Helper para evitar repetição de código
    private CheckInResponseDto MapearDto(TransacaoEQPoint t, int saldoTotal)
    {
      return new CheckInResponseDto
      {
        IdTransacao = t.IdTransacao,
        UsuarioId = t.UsuarioId,
        PontosGanhos = t.Valor,
        NovoSaldoTotal = saldoTotal,
        DataCheckIn = t.DataTransacao,
        Descricao = t.Descricao
      };
    }
  }
}
