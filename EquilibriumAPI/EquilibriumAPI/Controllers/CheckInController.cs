using Microsoft.AspNetCore.Mvc;
using EquilibriumModel;
using EquilibriumApplication.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EquilibriumAPI.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class CheckInController : ControllerBase
  {
    private readonly ITransacaoService _transacaoService;

    public CheckInController(ITransacaoService transacaoService)
    {
      _transacaoService = transacaoService;
    }

    // 1. CREATE (POST)
    [HttpPost]
    [ProducesResponseType(typeof(CheckInResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegistrarCheckIn([FromBody] CheckInRequestDto request)
    {
      try
      {
        var response = await _transacaoService.RegistrarCheckInAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    // 2. READ ALL (GET /usuario/{id}) - NOVO
    [HttpGet("usuario/{usuarioId}")]
    [ProducesResponseType(typeof(IEnumerable<CheckInResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistorico(int usuarioId)
    {
      var historico = await _transacaoService.GetHistoricoUsuarioAsync(usuarioId);
      return Ok(historico);
    }

    // 3. READ ONE (GET /{id}) - NOVO
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CheckInResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCheckInPorId(Guid id)
    {
      var checkIn = await _transacaoService.GetCheckInByIdAsync(id);
      if (checkIn == null) return NotFound("Check-in não encontrado.");
      return Ok(checkIn);
    }

    // 4. UPDATE (PUT /{id})
    // Atualiza o nível de humor. O ID vai na URL, o novo nível vai na Query string para simplificar
    // Ex: PUT /api/v1/checkin/aaaa-bbbb-cccc?novoNivel=5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AtualizarCheckIn(Guid id, [FromBody] AtualizarCheckInRequestDto request)
    {
      // O [ApiController] valida o Range(1,5) automaticamente aqui

      // Passamos o valor de dentro do DTO para o serviço
      var sucesso = await _transacaoService.AtualizarCheckInAsync(id, request.NovoNivelHumor);

      if (!sucesso) return NotFound("Check-in não encontrado.");

      return NoContent();
    }

    // 5. DELETE (DELETE /{id})
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletarCheckIn(Guid id)
    {
      var sucesso = await _transacaoService.DeletarCheckInAsync(id);

      if (!sucesso) return NotFound("Check-in não encontrado.");

      return NoContent();
    }
  }
}
