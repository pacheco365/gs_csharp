using Microsoft.AspNetCore.Mvc;
using EquilibriumApplication.Services; // IUsuarioService
using EquilibriumModel; // DTOs
using System;
using System.Threading.Tasks;

namespace EquilibriumAPI.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")] // Rota: /api/v1/usuarios
  public class UsuariosController : ControllerBase
  {
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
      _usuarioService = usuarioService;
    }

    // --- 1. CREATE ---
    // POST /api/v1/usuarios
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioRequestDto novoUsuarioDto)
    {
      try
      {
        var usuarioCriado = await _usuarioService.CriarNovoUsuarioAsync(novoUsuarioDto);
        return CreatedAtAction(nameof(GetUsuarioPorId),
                               new { id = usuarioCriado.IdUsuario },
                               usuarioCriado);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message); // Ex: "E-mail já existe"
      }
    }

    // --- 2. READ (Get All) ---
    // GET /api/v1/usuarios
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTodosUsuarios()
    {
      var usuarios = await _usuarioService.GetAllUsuariosAsync();
      return Ok(usuarios);
    }

    // --- 3. READ (Get By ID) ---
    // GET /api/v1/usuarios/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsuarioPorId(int id)
    {
      var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
      if (usuario == null)
      {
        return NotFound();
      }
      return Ok(usuario);
    }

    // --- 4. UPDATE ---
    // PUT /api/v1/usuarios/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] AtualizarUsuarioRequestDto usuarioDto)
    {
      try
      {
        var resultado = await _usuarioService.AtualizarUsuarioAsync(id, usuarioDto);
        if (!resultado)
        {
          return NotFound(); // Não encontrou o usuário
        }
        return NoContent(); // Sucesso
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message); // Ex: "E-mail já existe"
      }
    }

    // --- 5. DELETE ---
    // DELETE /api/v1/usuarios/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletarUsuario(int id)
    {
      var resultado = await _usuarioService.DeletarUsuarioAsync(id);
      if (!resultado)
      {
        return NotFound(); // Não encontrou o usuário
      }
      return NoContent(); // Sucesso
    }
  }
}
