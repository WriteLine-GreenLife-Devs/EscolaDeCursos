using EscolaDeCursos.Dominio.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAluno;

public interface IRepositorioAluno : IRepositorio<Aluno>
{
    void CadastrarAluno(
        Usuario usuario,
        Aluno aluno
    );
}