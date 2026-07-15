using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloAluno;

public sealed class RepositorioPresencaAluno(
    EscolaDeCursosDbContext dbContext)
    : RepositorioBase<PresencaAluno>(dbContext),
      IRepositorioPresencaAluno
{
    private readonly EscolaDeCursosDbContext contexto = dbContext;

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

    public void SalvarAlteracoes(List<PresencaAluno> presencasNovas)
    {
        registros.AddRange(presencasNovas);
        contexto.SaveChanges();
    }
}