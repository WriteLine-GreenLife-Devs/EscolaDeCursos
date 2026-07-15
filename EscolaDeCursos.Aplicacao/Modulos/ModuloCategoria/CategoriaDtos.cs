using System.ComponentModel.DataAnnotations;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloCategoria;

public sealed class CadastrarCategoriaDto
{
    public CadastrarCategoriaDto()
    {
    }

    public CadastrarCategoriaDto(string nome, string descricao, StatusCategoria status)
    {
        this.nome = nome;
        this.descricao = descricao;
        this.status = status;
    }

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
    public EditarCategoriaDto()
    {
    }

    public EditarCategoriaDto(Guid id, string nome, string descricao, StatusCategoria status)
    {
        Id = id;
        this.nome = nome;
        this.descricao = descricao;
        this.status = status;
    }

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
    public ExcluirCategoriaDto()
    {
    }

    public ExcluirCategoriaDto(Guid id, string nome, string descricao, StatusCategoria status)
    {
        Id = id;
        this.nome = nome;
        this.descricao = descricao;
        this.status = status;
    }

    public Guid Id { get; set; }
    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
    public StatusCategoria status { get; set; } = StatusCategoria.Ativo;
}
