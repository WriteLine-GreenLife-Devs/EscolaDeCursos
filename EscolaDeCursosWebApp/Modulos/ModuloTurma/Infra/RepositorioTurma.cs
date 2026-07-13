using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloTurma.Infra;

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
