using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloUsuario.Infra;

public sealed class RepositorioUsuario(EscolaDeCursosDbContext dbContext) :
    RepositorioBase<Usuario>(dbContext), IRepositorioUsuario
{
    public override List<Usuario> SelecionarTodos()
    {
        return registros.OrderBy(c => c.nome).ToList();
    }

    public override List<Usuario> Filtrar(Func<Usuario, bool> filtro)
    {
        return registros.Where(filtro).OrderBy(c => c.nome).ToList();
    }
}
