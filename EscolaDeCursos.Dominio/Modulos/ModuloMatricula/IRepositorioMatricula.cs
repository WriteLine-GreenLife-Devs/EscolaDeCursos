using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloMatricula;

public interface IRepositorioMatricula : IRepositorio<Matricula>
{
    ResultadoCadastroMatricula CadastrarComControleDeVagas(
        Matricula matricula,
        int vagasMaximas);
}