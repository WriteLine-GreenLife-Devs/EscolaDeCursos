using EscolaDeCursos.Dominio.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;

namespace EscolaDeCursos.Dominio.Modulos.ModuloProfessor;

public interface IRepositorioProfessor
    : IRepositorio<Professor>
{
    void CadastrarProfessor(
        Usuario usuario,
        Professor professor
    );
}
