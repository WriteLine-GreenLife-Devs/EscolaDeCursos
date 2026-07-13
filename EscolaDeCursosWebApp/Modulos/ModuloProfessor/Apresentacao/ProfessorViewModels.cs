using System.ComponentModel.DataAnnotations;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Apresentacao;

public record CadastrarProfessorPerfilViewModel(
    [Required(ErrorMessage = "O usuário é obrigatório.")]
    Guid UsuarioId,

    [Required(ErrorMessage = "A biografia é obrigatória.")]
    [StringLength(
        1000,
        ErrorMessage = "A biografia deve conter no máximo 1000 caracteres."
    )]
    string Bio,

    [Required(ErrorMessage = "As especialidades são obrigatórias.")]
    [StringLength(
        500,
        ErrorMessage = "As especialidades devem conter no máximo 500 caracteres."
    )]
    string Especialidades,

    [Required(ErrorMessage = "A data de contratação é obrigatória.")]
    [DataType(DataType.Date)]
    DateTime DataContratacao
);

public record EditarProfessorPerfilViewModel(
    Guid Id,

    [Required(ErrorMessage = "A biografia é obrigatória.")]
    [StringLength(
        1000,
        ErrorMessage = "A biografia deve conter no máximo 1000 caracteres."
    )]
    string Bio,

    [Required(ErrorMessage = "As especialidades são obrigatórias.")]
    [StringLength(
        500,
        ErrorMessage = "As especialidades devem conter no máximo 500 caracteres."
    )]
    string Especialidades,

    [Required(ErrorMessage = "A data de contratação é obrigatória.")]
    [DataType(DataType.Date)]
    DateTime DataContratacao
);

public record ListarProfessoresViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Especialidades,
    DateTime DataContratacao
);

public record DetalhesProfessorViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string Bio,
    string Especialidades,
    DateTime DataContratacao
);

public sealed class PainelProfessorViewModel
{
    public string NomeProfessor { get; init; } = string.Empty;
    public List<TurmaProfessorViewModel> Turmas { get; init; } = [];
}

public sealed record TurmaProfessorViewModel(
    Guid Id,
    string Nome,
    string CursoNome,
    DateTime DataInicio,
    DateTime DataFim,
    string HorarioTurno,
    StatusTurma Status,
    int TotalAlunos
);

public sealed class DetalhesTurmaProfessorViewModel
{
    public Guid TurmaId { get; init; }
    public string TurmaNome { get; init; } = string.Empty;
    public int VagasMaximas { get; init; }
    public List<AlunoTurmaProfessorViewModel> Alunos { get; init; } = [];
}

public sealed record AlunoTurmaProfessorViewModel(
    Guid MatriculaId,
    string Nome,
    DateTime DataMatricula,
    SituacaoMatricula Situacao
);
