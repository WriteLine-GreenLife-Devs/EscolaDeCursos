using EscolaDeCursosWebApp.Compartilhado.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;

public interface IRepositorioPresencaAluno : IRepositorio<PresencaAluno>
{
    void SalvarAlteracoes(List<PresencaAluno> presencasNovas);
}