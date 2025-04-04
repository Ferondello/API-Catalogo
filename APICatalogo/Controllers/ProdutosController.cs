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
        private readonly IProdutoRepository _produtoRepository;
        private readonly IRepository<Produto> _repository;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoRepository produtoRepository, IRepository<Produto> repository, ILogger<ProdutosController> logger)
        {
            _produtoRepository = produtoRepository;
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("Produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos  = _produtoRepository.GetProdutosPorCategoria(id);
            if(produtos is null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetAll();
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
            var produto = _repository.Get(c=> c.ProdutoId == id);
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

            var produtoAtualizado =_repository.Update(produto);
            return Ok(produtoAtualizado);


        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _repository.Get(p=> p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            var produtoDeletado = _repository.Delete(produto);
            return Ok(produtoDeletado);

        }
    }
}
