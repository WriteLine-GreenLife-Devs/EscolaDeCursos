using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;

public record CadastrarUsuarioDto(
    string nome,
    string email,
    string senha,
    string telefone,
    TipoUsuario tipoUsuario
);
