using EscolaDeCursos.Dominio.Modulos.ModuloConteudoCurso;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using EntidadeModuloCurso = EscolaDeCursos.Dominio.Modulos.ModuloConteudoCurso.ModuloCurso;

namespace EscolaDeCursos.Infra.Modulos.ModuloConteudoCurso;

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