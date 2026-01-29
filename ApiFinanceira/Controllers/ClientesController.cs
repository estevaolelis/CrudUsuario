using System.Collections.Generic;
using System.Threading.Tasks;
using ApiFinanceira.Models;
using ApiFinanceira.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanceira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesService _clienteService;
        private readonly ExportacaoService _exportacaoService;

        public ClientesController(ClientesService clienteService,  ExportacaoService exportacaoService)
        {
            _clienteService = clienteService;
            _exportacaoService = exportacaoService;
        }

        [HttpGet("listar-usuarios")]
        public async Task<IActionResult> ListarClientes()
        {
            var lista = await _clienteService.GetClientesAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterClientePorId(int id)
        {
            var cliente = await _clienteService.GetClientesByIdAsync(id);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado." });
            }

            return Ok(cliente);
        }

        [HttpPost("criar-cliente")]
        public async Task<IActionResult> CriarClientes([FromBody] ClienteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var novoCliente = await _clienteService.PostClientesAsync(request.Nome, request.Documento, request.Email);
            
            return CreatedAtAction(nameof(ObterClientePorId), new { id = novoCliente.Id }, novoCliente);
        }

        [HttpPut("atualizar-cliente/{id}")]
        public async Task<IActionResult> AtualizarCliente(int id, [FromBody] ClienteRequest request)
        {
            var atualizado = await _clienteService.PutClientesAsync(id, request.Nome, request.Documento, request.Email);

            if (atualizado == null)
            {
                return NotFound(new { message = "Cliente não encontrado para atualização." });
            }

            return Ok(atualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCliente(int id)
        {
            var sucesso = await _clienteService.DeleteClientesAsync(id);

            if (!sucesso)
            {
                return NotFound(new { message = "Cliente não encontrado." });
            }

            return NoContent();
        }
        
        [HttpGet("clientes")]
        public async Task<IActionResult> ExportarClientes()
        {
            var dados = await _clienteService.GetClientesAsync();
            
            var config = new List<CsvColunaConfiguracao<Cliente>>
            {
                new() { Cabecalho = "ID", Formatador = c => c.Id.ToString() },
                new() { Cabecalho = "Nome Completo", Formatador = c => c.Nome },
                new() { Cabecalho = "Documento", Formatador = c => c.Documento },
                new() { Cabecalho = "E-mail", Formatador = c => c.Email },
                new() { Cabecalho = "Status", Formatador = c => c.Status ? "Ativo" : "Inativo" },
                new() { Cabecalho = "Data Criação", Formatador = c => c.DataCriacao.ToString("dd/MM/yyyy HH:mm") }
            };
            
            var arquivo = _exportacaoService.ExportarPlanilha(dados, config, FormatoExportacao.xlsx);
            
            return File(arquivo.Conteudo, arquivo.ContentType, arquivo.NomeArquivo);
        }
    }


    public class ClienteRequest
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
    }
}