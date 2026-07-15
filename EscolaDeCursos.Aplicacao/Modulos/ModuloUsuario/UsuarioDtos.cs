using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloUsuario;

public record CadastrarUsuarioDto(
    string nome,
    string email,
    string senha,
    string telefone,
    TipoUsuario tipoUsuario
);