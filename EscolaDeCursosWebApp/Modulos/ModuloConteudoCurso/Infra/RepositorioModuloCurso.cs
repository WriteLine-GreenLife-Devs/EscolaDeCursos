using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio;
using EntidadeModuloCurso = EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio.ModuloCurso;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Infra;

public sealed class RepositorioModuloCurso(
    EscolaDeCursosDbContext dbContext)
    : RepositorioBase<EntidadeModuloCurso>(dbContext),
      IRepositorioModuloCurso
{
    public override List<EntidadeModuloCurso> SelecionarTodos()
    {
        return registros
            .OrderBy(modulo => modulo.CursoId)
            .ThenBy(modulo => modulo.Ordem)
            .ToList();
    }

    public override List<EntidadeModuloCurso> Filtrar(
        Func<EntidadeModuloCurso, bool> filtro)
    {
        return registros
            .Where(filtro)
            .OrderBy(modulo => modulo.Ordem)
            .ToList();
    }
}