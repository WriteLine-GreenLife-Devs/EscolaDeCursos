using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

public sealed record PainelADMViewModel(
    int TotalCursosAtivos,
    int TotalTurmas,
    int TotalAlunosAtivos,
    int TotalProfessoresAtivos,
    int TotalMatriculasCursando
);

public sealed class DetalhesAlunoADMViewModel
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public bool Ativo { get; init; }
    public List<MatriculaAlunoADMViewModel> Matriculas { get; init; } = [];
}

public sealed record MatriculaAlunoADMViewModel(
    Guid Id,
    Guid TurmaId,
    string CursoNome,
    string TurmaNome,
    DateTime DataMatricula,
    SituacaoMatricula Situacao,
    double? Nota1,
    double? Nota2,
    double? Nota3,
    double? Recuperacao,
    double? NotaFinal,
    int FrequenciaPercentual,
    int TotalPresencas,
    int TotalPresentes,
    ResumoProgressoModuloAlunoViewModel? ProgressoModulos,
    List<PresencaAlunoADMViewModel> Presencas
);

public sealed record PresencaAlunoADMViewModel(
    DateTime Data,
    bool Presente
);