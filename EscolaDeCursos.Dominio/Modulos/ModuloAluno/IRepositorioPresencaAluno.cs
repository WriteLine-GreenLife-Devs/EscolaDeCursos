using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAluno;

public interface IRepositorioPresencaAluno : IRepositorio<PresencaAluno>
{
    void SalvarAlteracoes(List<PresencaAluno> presencasNovas);
}