using System.ComponentModel.DataAnnotations;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;

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
    public DetalhesProfessorViewModel? Perfil { get; init; }
    public bool PerfilCadastrado { get; init; }
    public List<CursoProfessorViewModel> Cursos { get; init; } = [];
}

public sealed record CursoProfessorViewModel(
    string Nome,
    int TotalAlunos,
    List<TurmaProfessorViewModel> Turmas
);

public sealed record TurmaProfessorViewModel(
    Guid Id,
    Guid CursoId,
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
    public DateTime DataInicio { get; init; }
    public DateTime DataFim { get; init; }
    public int VagasMaximas { get; init; }
    public ModulosCursoParcialViewModel ModulosCurso { get; init; } = new();
    public List<AlunoTurmaProfessorViewModel> Alunos { get; init; } = [];
    public List<ChamadaProfessorViewModel> Chamadas { get; init; } = [];
}

public sealed record AlunoTurmaProfessorViewModel(
    Guid MatriculaId,
    Guid AlunoId,
    string Nome,
    string Email,
    string Telefone,
    bool Ativo,
    DateTime DataMatricula,
    SituacaoMatricula Situacao,
    double? Nota1,
    double? Nota2,
    double? Nota3,
    double? Recuperacao,
    double? NotaFinal,
    ResumoProgressoModuloAlunoViewModel? ProgressoModulos,
    List<MatriculaAlunoProfessorViewModel> Matriculas
);

public sealed record MatriculaAlunoProfessorViewModel(
    Guid Id,
    string CursoNome,
    string TurmaNome,
    SituacaoMatricula Situacao,
    int FrequenciaPercentual,
    int TotalPresencas,
    int TotalPresentes,
    List<PresencaAlunoProfessorViewModel> Presencas
);

public sealed record PresencaAlunoProfessorViewModel(
    DateTime Data,
    bool Presente
);

public sealed record ChamadaProfessorViewModel(
    DateTime Data,
    List<AlunoChamadaProfessorViewModel> Alunos
);

public sealed record AlunoChamadaProfessorViewModel(
    Guid MatriculaId,
    string Nome,
    bool? Presente
);

public sealed class SalvarChamadaProfessorViewModel
{
    [Required]
    public Guid TurmaId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DataAula { get; set; }

    public List<PresencaChamadaProfessorViewModel> Alunos { get; set; } = [];
}

public sealed class PresencaChamadaProfessorViewModel
{
    [Required]
    public Guid MatriculaId { get; set; }

    public bool Presente { get; set; }
}

public sealed class SalvarNotasProfessorViewModel
{
    [Required]
    public Guid MatriculaId { get; set; }

    [Required]
    public Guid TurmaId { get; set; }

    [Range(0, 10)]
    public double? Nota1 { get; set; }

    [Range(0, 10)]
    public double? Nota2 { get; set; }

    [Range(0, 10)]
    public double? Nota3 { get; set; }

    [Range(0, 10)]
    public double? Recuperacao { get; set; }
}