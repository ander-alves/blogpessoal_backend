using blogPessoal.Model;

namespace blogPessoal.Service
{
    public interface IPostagemService
    {
        Task<IEnumerable<Postagem>> GetAll();
        Task<Postagem?> GetById(int id);
        Task<IEnumerable<Postagem>> GetByTitulo(string titulo);
        Task<Postagem?> Create(Postagem postagem);
        Task<Postagem?> Update(Postagem postagem);
        Task Delete(int id);

    }
}
