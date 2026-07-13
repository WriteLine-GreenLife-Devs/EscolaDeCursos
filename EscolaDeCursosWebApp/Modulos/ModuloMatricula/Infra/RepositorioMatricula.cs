using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloMatricula.Infra;

public sealed class RepositorioMatricula(EscolaDeCursosDbContext dbContext) :
    RepositorioBase<Matricula>(dbContext), IRepositorioMatricula
{
    public override List<Matricula> SelecionarTodos()
    {
        return registros.OrderBy(m => m.DataMatricula).ToList();
    }

    public override List<Matricula> Filtrar(Func<Matricula, bool> filtro)
    {
        return registros.Where(filtro).OrderBy(m => m.DataMatricula).ToList();
    }
}
