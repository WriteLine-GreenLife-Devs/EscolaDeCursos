using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Infra;

public sealed class RepositorioNotaAluno(
    EscolaDeCursosDbContext dbContext)
    : RepositorioBase<NotaAluno>(dbContext),
      IRepositorioNotaAluno
{
    public override List<NotaAluno> SelecionarTodos()
    {
        return registros
            .OrderBy(nota => nota.DataLancamento)
            .ThenBy(nota => nota.TipoNota)
            .ToList();
    }

    public override List<NotaAluno> Filtrar(
        Func<NotaAluno, bool> filtro)
    {
        return registros
            .Where(filtro)
            .OrderBy(nota => nota.DataLancamento)
            .ThenBy(nota => nota.TipoNota)
            .ToList();
    }
}