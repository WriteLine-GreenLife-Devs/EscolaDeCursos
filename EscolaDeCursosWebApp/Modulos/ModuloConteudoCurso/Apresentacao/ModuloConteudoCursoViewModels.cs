using System.ComponentModel.DataAnnotations;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;

public class CadastrarModuloCursoViewModel
{
    [Required(ErrorMessage = "O curso é obrigatório.")]
    public Guid CursoId { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "O título deve conter entre 3 e 100 caracteres."
    )]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(
        1000,
        MinimumLength = 3,
        ErrorMessage = "A descrição deve conter entre 3 e 1000 caracteres."
    )]
    public string Descricao { get; set; } = string.Empty;

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A duração deve ser maior que zero."
    )]
    public int DuracaoMinutos { get; set; }

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A ordem deve ser maior que zero."
    )]
    public int Ordem { get; set; }
}

public sealed class EditarModuloCursoViewModel
    : CadastrarModuloCursoViewModel
{
    public Guid Id { get; set; }
}

public sealed record ModuloCursoViewModel(
    Guid Id,
    Guid CursoId,
    string Titulo,
    string Descricao,
    int DuracaoMinutos,
    int Ordem,
    bool Ativo
);

public sealed class ListaModulosCursoViewModel
{
    public Guid CursoId { get; init; }
    public List<ModuloCursoViewModel> Modulos { get; init; } = [];
}

public sealed class AtualizarConclusaoModuloAlunoViewModel
{
    [Required]
    public Guid MatriculaId { get; set; }

    [Required]
    public Guid ModuloCursoId { get; set; }

    public bool Concluido { get; set; }
}

public sealed record ModuloProgressoAlunoViewModel(
    Guid ModuloCursoId,
    string Titulo,
    string Descricao,
    int DuracaoMinutos,
    int Ordem,
    bool Concluido,
    DateTime? DataConclusao
);

public sealed record ResumoProgressoModuloAlunoViewModel(
    Guid MatriculaId,
    int TotalModulos,
    int ModulosConcluidos,
    long DuracaoTotalMinutos,
    long DuracaoConcluidaMinutos,
    int PercentualConclusao,
    List<ModuloProgressoAlunoViewModel> Modulos
);