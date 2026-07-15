using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloCurso;

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
