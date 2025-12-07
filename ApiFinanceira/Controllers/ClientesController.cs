using ApiFinanceira.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanceira.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly Supabase.Client _supabase;
    private readonly ClientesService _clienteService;

    public ClientesController(Supabase.Client supabase, ClientesService clienteService)
    {
        _supabase = supabase;
        _clienteService = clienteService;
    }

    [HttpGet("listar-usuarios")]
    public async Task<IActionResult> ListarUsuarios()
    {
        var listar = await _clienteService.GetClientesAsync();
        return Ok(listar);
    }
    
    [HttpPost("criar-usuarios")]
    public async Task<IActionResult> CriarUsuarios(string nome, string documento, string email)
    {
        var listar = await _clienteService.PostClientesAsync(nome, documento, email);
        return Ok(listar);
    }
}

