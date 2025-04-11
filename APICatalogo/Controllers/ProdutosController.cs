using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
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
        private readonly IUnityOfWork _uof;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(ILogger<ProdutosController> logger, IUnityOfWork uof)
        { 
            _logger = logger;
            _uof = uof;
        }
        [HttpGet("Produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos  = _uof.ProdutoRepository.GetProdutosPorCategoria(id);
            if(produtos is null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _uof.CategoriaRepository.GetAll();
            if (produtos is null)
            {
                _logger.LogWarning("Produto não encontrado...");
                return NotFound("Produto não encontrado...");
            }
            return Ok(produtos);

        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uof.ProdutoRepository.Get(c=> c.ProdutoId == id);
            if (produto is null)
            {
                _logger.LogWarning($"Produto de ID= {id} não encontrado...");
                return NotFound($"Produto de ID= {id} não encontrado...");
            }
            return Ok(produto);

        }
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados incálidos...");
            }
            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();
            return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning($"Nenhum produto com ID= {id}");
                return BadRequest($"Nenhum produto com ID= {id}");//400
            }

            var produtoAtualizado =_uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok(produtoAtualizado);


        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p=> p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return Ok(produtoDeletado);

        }
    }
}
