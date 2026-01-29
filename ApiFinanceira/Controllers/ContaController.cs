using System.Threading.Tasks;
using ApiFinanceira.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanceira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly ContaService _contaService;

        public ContaController(ContaService contaService)
        {
            _contaService = contaService;
        }

        [HttpGet("listar-contas")]
        public async Task<IActionResult> ListarTodasContas()
        {
            var lista = await _contaService.GetContasAsync();
            return Ok(lista);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ListarContasPorUsuario(int usuarioId)
        {
            var lista = await _contaService.GetContasPorUsuarioAsync(usuarioId);
            return Ok(lista);
        }

        [HttpPost("criar-conta")]
        public async Task<IActionResult> CriarConta([FromBody] CriarContaRequest request)
        {
            var novaConta = await _contaService.PostContaAsync(request.UsuarioId, request.TipoConta, request.SaldoInicial);
            return Ok(novaConta);
        }
    }

    public class CriarContaRequest
    {
        public int UsuarioId { get; set; }
        public string TipoConta { get; set; }
        public decimal SaldoInicial { get; set; }
    }
}