using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        [HttpGet("LerArquivoConfiguracao")]
        public string GetValores()
        {
            var valor1 = _configuration ["chave1"];
            var valor2 = _configuration["chave2"];

            var secao1 = _configuration["secao1:chave2"];
            return $"Chave1 = {valor1}  \nChave2 = {valor2}   \n Seção1 => Chave2 ={secao1}";
        }

        [HttpGet("usandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromService([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
        {
            try
            {
                _logger.LogInformation("================= GET api/categorias/produtos ===============");
                return await _context.Categorias.Include(p => p.Produtos).ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao realizar sua solicitação.");
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            try
            {
                _logger.LogInformation("================= GET api/categorias ===============");
                return await _context.Categorias.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao realizar a solicitação.");
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async  Task<ActionResult<Categoria>> GetByID(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(p => p.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound("Categoria nao encontrado");
                }
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado ao realizar a solicitação");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest();
                }
                await _context.Categorias.AddAsync(categoria);
                await _context.SaveChangesAsync();
                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema a tratar sua solicitação.");
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest();
                }

                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado ao realizar a inserção.");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Categoria>> Delete(int id) 
        {
            try
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaId.Equals(id));
                if (categoria is null)
                {
                    return NotFound();
                }
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado ao realizar a deleção.");
            }
        }
    }
}
