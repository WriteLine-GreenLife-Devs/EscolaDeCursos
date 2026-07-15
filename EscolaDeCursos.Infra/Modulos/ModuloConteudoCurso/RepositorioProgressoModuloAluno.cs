using EscolaDeCursos.Infra.Compartilhado.Orm;
using EscolaDeCursos.Dominio.Modulos.ModuloConteudoCurso;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Infra;

public sealed class RepositorioProgressoModuloAluno(
    EscolaDeCursosDbContext dbContext)
    : RepositorioBase<ProgressoModuloAluno>(dbContext),
      IRepositorioProgressoModuloAluno
{
    public override List<ProgressoModuloAluno> SelecionarTodos()
    {
        return registros
            .OrderBy(progresso => progresso.MatriculaId)
            .ThenBy(progresso => progresso.ModuloCursoId)
            .ToList();
    }

    public override List<ProgressoModuloAluno> Filtrar(
        Func<ProgressoModuloAluno, bool> filtro)
    {
        return registros
            .Where(filtro)
            .OrderBy(progresso => progresso.ModuloCursoId)
            .ToList();
    }
}