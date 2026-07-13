using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloCurso.Infra;

public sealed class RepositorioCurso(EscolaDeCursosDbContext dbContext) :
    RepositorioBase<Curso>(dbContext), IRepositorioCurso
{
    public override List<Curso> SelecionarTodos()
    {
        return registros.OrderBy(c => c.nome).ToList();
    }

    public override List<Curso> Filtrar(Func<Curso, bool> filtro)
    {
        return registros.Where(filtro).OrderBy(c => c.nome).ToList();
    }
}
