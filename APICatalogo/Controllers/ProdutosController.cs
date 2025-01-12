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

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            try
            {
                var produtos = await _context.Produtos.ToListAsync();
                if (produtos is null)
                {
                    return NotFound();
                }
                return produtos;
            }
            catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado ao realizar a solicitação."); 
            }
        }

        [HttpGet("{id:int}", Name="ObterProduto")]
        public async Task<ActionResult<Produto>> GetByID([FromQuery] int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto nao encontrado");
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado ao realizar a solicitação");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            try
            {
                if (produto is null)
                {
                    return BadRequest();
                }
                await _context.Produtos.AddAsync(produto);
                await _context.SaveChangesAsync();
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado ao criar produto");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }

                _context.Entry(produto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(produto);
            }
            catch (Exception) { return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado ao realizar a inserção."); }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto nao localizado");
                }

                _context.Produtos.Remove(produto);
               await _context.SaveChangesAsync();
                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado ao realizar a deleção");
            }
        }
    }
}
