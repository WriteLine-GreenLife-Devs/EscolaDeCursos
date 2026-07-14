using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Infra;

public sealed class RepositorioPresencaAluno(
    EscolaDeCursosDbContext dbContext)
    : RepositorioBase<PresencaAluno>(dbContext),
      IRepositorioPresencaAluno
{
    public override List<PresencaAluno> SelecionarTodos()
    {
        return registros
            .OrderBy(presenca => presenca.DataAula)
            .ToList();
    }

    public override List<PresencaAluno> Filtrar(
        Func<PresencaAluno, bool> filtro)
    {
        return registros
            .Where(filtro)
            .OrderBy(presenca => presenca.DataAula)
            .ToList();
    }
}