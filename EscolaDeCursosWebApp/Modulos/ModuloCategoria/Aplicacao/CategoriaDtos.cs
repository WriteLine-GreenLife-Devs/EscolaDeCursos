using System.ComponentModel.DataAnnotations;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloCategoria.Aplicacao;

public sealed class CadastrarCategoriaDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string nome { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string descricao { get; set; } = string.Empty;

    [Required]
    public StatusCategoria status { get; set; } = StatusCategoria.Ativo;
}

public sealed class EditarCategoriaDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string nome { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string descricao { get; set; } = string.Empty;

    [Required]
    public StatusCategoria status { get; set; } = StatusCategoria.Ativo;
}

public sealed class ExcluirCategoriaDto
{
    public Guid Id { get; set; }
    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
    public StatusCategoria status { get; set; } = StatusCategoria.Ativo;
}
