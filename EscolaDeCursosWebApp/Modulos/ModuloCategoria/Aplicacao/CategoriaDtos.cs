using System.ComponentModel.DataAnnotations;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloCategoria.Aplicacao;

public record CadastrarCategoriaDto(
    [property: Required]
    [property: StringLength(100, MinimumLength = 3)]
    string nome,

    [property: Required]
    [property: StringLength(200, MinimumLength = 3)]
    string descricao,

    [property: Required]
    StatusCategoria status
);

public record EditarCategoriaDto(
    Guid Id,

    [property: Required]
    [property: StringLength(100, MinimumLength = 3)]
    string nome,

    [property: Required]
    [property: StringLength(200, MinimumLength = 3)]
    string descricao,

    [property: Required]
    StatusCategoria status
);

public record ExcluirCategoriaDto(
    Guid Id,
    string nome,
    string descricao,
    StatusCategoria status
);
