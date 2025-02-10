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
        private readonly IProdutoRepository _repository;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoRepository repository, ILogger<ProdutosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetProdutos().ToList();
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
            var produto = _repository.GetProduto(id);
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
            var novoProduto = _repository.Create(produto);
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

            bool atualizado =_repository.Update(produto);
            if(atualizado)
            {
                return Ok(produto);
            }
            else
            {
                return StatusCode(500, $"Falha ao atualiazr o produto de ID = {id}");
            }

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            bool deletado = _repository.Delete(id);
            if (deletado)
            {
                return Ok($"Produto de ID ={id} foi excluido");
            }
            else
            {
                return StatusCode(500, $"Falha ao excluir produto de ID = {id}");
            }

        }
    }
}
