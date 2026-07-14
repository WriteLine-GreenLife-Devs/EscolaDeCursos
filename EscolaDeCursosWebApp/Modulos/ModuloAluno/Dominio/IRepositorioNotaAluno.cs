using EscolaDeCursosWebApp.Compartilhado.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;

public interface IRepositorioNotaAluno : IRepositorio<NotaAluno>
{
    void SalvarAlteracoes(
        List<NotaAluno> notasNovas,
        List<NotaAluno> notasRemovidas);
}