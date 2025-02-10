using APICatalogo.Models;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository
    {
        IQueryable<Produto> GetProdutos();
        Produto GetProduto(int id);
        Produto Create(Produto produto);
        bool Update(Produto produto);
        bool Delete(int id);

    }
}
