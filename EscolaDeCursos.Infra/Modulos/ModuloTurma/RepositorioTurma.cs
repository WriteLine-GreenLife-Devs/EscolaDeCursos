using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloTurma;

public sealed class RepositorioTurma(EscolaDeCursosDbContext dbContext) :
    RepositorioBase<Turma>(dbContext), IRepositorioTurma
{
    public override List<Turma> SelecionarTodos()
    {
        return registros.OrderBy(t => t.nome).ToList();
    }

    public override List<Turma> Filtrar(Func<Turma, bool> filtro)
    {
        return registros.Where(filtro).OrderBy(t => t.nome).ToList();
    }
}
