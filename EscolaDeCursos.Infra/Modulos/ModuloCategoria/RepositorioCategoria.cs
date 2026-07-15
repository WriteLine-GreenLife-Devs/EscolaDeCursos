using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloCategoria;

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
