using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloCategoria.Infra;

public sealed class RepositorioCategoria(EscolaDeCursosDbContext dbContext) :
    RepositorioBase<Categoria>(dbContext), IRepositorioCategoria
{
    public override List<Categoria> SelecionarTodos()
    {
        return registros.OrderBy(c => c.nome).ToList();
    }

    public override List<Categoria> Filtrar(Func<Categoria, bool> filtro)
    {
        return registros.Where(filtro).OrderBy(c => c.nome).ToList();
    }
}
