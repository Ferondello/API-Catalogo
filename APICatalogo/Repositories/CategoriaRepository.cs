using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Categoria Create(Categoria categoria)
        {
            if(categoria is null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Delete(int id)
        {
            var cat = _context.Categorias.Find(id);
            if(cat == null)
            {
                throw new ArgumentNullException("Categoria não existe");
            }
            _context.Categorias.Remove(cat);
            _context.SaveChanges();
            return cat;

        }

        public Categoria GetCategoria(int id)
        {
           return _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            var categorias = _context.Categorias.ToList();

            return categorias;
        }

        public Categoria Update(Categoria categoria)
        {
            if(categoria is null)
            {
                throw new ArgumentNullException("Categoria nao existe...");
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }
    }
}
