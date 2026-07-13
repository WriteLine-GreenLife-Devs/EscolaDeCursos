using System.ComponentModel.DataAnnotations;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloCurso.Aplicacao;

public sealed class CadastrarCursoDto
{
    public CadastrarCursoDto()
    {
    }

    public CadastrarCursoDto(
        string nome,
        string descricao,
        int cargaHoraria,
        NivelDificuldade nivelDificuldade,
        StatusCurso status,
        decimal valor,
        Guid categoriaId
    )
    {
        this.nome = nome;
        this.descricao = descricao;
        this.cargaHoraria = cargaHoraria;
        this.nivelDificuldade = nivelDificuldade;
        this.status = status;
        this.valor = valor;
        this.categoriaId = categoriaId;
    }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string nome { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string descricao { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A carga horária deve ser maior que zero.")]
    public int cargaHoraria { get; set; } = 1;

    [Required]
    public NivelDificuldade nivelDificuldade { get; set; } = NivelDificuldade.Iniciante;

    [Required]
    public StatusCurso status { get; set; } = StatusCurso.Ativo;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "O valor do curso não pode ser negativo.")]
    public decimal valor { get; set; } = 0;

    [Required(ErrorMessage = "A categoria do curso é obrigatória.")]
    public Guid categoriaId { get; set; }
}

public sealed class EditarCursoDto
{
    public EditarCursoDto()
    {
    }

    public EditarCursoDto(
        Guid id,
        string nome,
        string descricao,
        int cargaHoraria,
        NivelDificuldade nivelDificuldade,
        StatusCurso status,
        decimal valor,
        Guid categoriaId
    )
    {
        Id = id;
        this.nome = nome;
        this.descricao = descricao;
        this.cargaHoraria = cargaHoraria;
        this.nivelDificuldade = nivelDificuldade;
        this.status = status;
        this.valor = valor;
        this.categoriaId = categoriaId;
    }

    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string nome { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string descricao { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A carga horária deve ser maior que zero.")]
    public int cargaHoraria { get; set; } = 1;

    [Required]
    public NivelDificuldade nivelDificuldade { get; set; } = NivelDificuldade.Iniciante;

    [Required]
    public StatusCurso status { get; set; } = StatusCurso.Ativo;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "O valor do curso não pode ser negativo.")]
    public decimal valor { get; set; } = 0;

    [Required(ErrorMessage = "A categoria do curso é obrigatória.")]
    public Guid categoriaId { get; set; }
}

public sealed class ExcluirCursoDto
{
    public ExcluirCursoDto()
    {
    }

    public ExcluirCursoDto(
        Guid id,
        string nome,
        string descricao,
        int cargaHoraria,
        NivelDificuldade nivelDificuldade,
        StatusCurso status,
        decimal valor,
        Guid categoriaId
    )
    {
        Id = id;
        this.nome = nome;
        this.descricao = descricao;
        this.cargaHoraria = cargaHoraria;
        this.nivelDificuldade = nivelDificuldade;
        this.status = status;
        this.valor = valor;
        this.categoriaId = categoriaId;
    }

    public Guid Id { get; set; }
    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
    public int cargaHoraria { get; set; } = 1;
    public NivelDificuldade nivelDificuldade { get; set; } = NivelDificuldade.Iniciante;
    public StatusCurso status { get; set; } = StatusCurso.Ativo;
    public decimal valor { get; set; } = 0;
    public Guid categoriaId { get; set; }
    public string CategoriaNome { get; set; } = string.Empty;
}
