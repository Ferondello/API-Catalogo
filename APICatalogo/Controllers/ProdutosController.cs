using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(AppDbContext context, ILogger<ProdutosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos.ToListAsync();
            if (produtos is null)
            {
                _logger.LogWarning("Produto não encontrado...");
                return NotFound("Produto não encontrado...");
            }
            return produtos;

        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetByID([FromQuery] int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
            if (produto is null)
            {
                _logger.LogWarning($"Produto de ID= {id} não encontrado...");
                return NotFound($"Produto de ID= {id} não encontrado...");
            }
            return produto;

        }
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados incálidos...");
            }
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning($"Nenhum produto com ID= {id}");
                return BadRequest($"Nenhum produto com ID= {id}");
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                _logger.LogWarning($"Nenhum produto encontrado com ID= {id}");
                return NotFound($"Nenhum produto encontrado com ID= {id}");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return Ok(produto);

        }
    }
}
