using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAluno;

public interface IRepositorioNotaAluno : IRepositorio<NotaAluno>
{
    void SalvarAlteracoes(
        List<NotaAluno> notasNovas,
        List<NotaAluno> notasRemovidas);
}