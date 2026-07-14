using EscolaDeCursosWebApp.Compartilhado.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;

public interface IRepositorioAluno : IRepositorio<Aluno>
{
    void CadastrarAluno(
        Usuario usuario,
        Aluno aluno
    );
}