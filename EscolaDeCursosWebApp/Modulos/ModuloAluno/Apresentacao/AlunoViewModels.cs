using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Apresentacao;

public record DetalhesAlunoViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    bool Ativo
);

public record TurmaResumoAlunoViewModel(
    Guid Id,
    string Nome,
    DateTime DataInicio,
    DateTime DataFim,
    string HorarioTurno,
    StatusTurma Status
);

public record CursoResumoAlunoViewModel(
    Guid Id,
    string Nome,
    string Descricao,
    int CargaHoraria,
    NivelDificuldade NivelDificuldade
);

public record ProfessorResumoAlunoViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Bio,
    string Especialidades
);

public record TurmaCatalogoAlunoViewModel(
    Guid Id,
    string Nome,
    DateTime DataInicio,
    DateTime DataFim,
    string HorarioTurno,
    StatusTurma Status,
    string ProfessorNome,
    int VagasDisponiveis,
    bool AlunoMatriculado,
    bool InscricaoDisponivel,
    string MotivoIndisponibilidade
);

public record CursoCatalogoAlunoViewModel(
    Guid Id,
    string Nome,
    string Descricao,
    int CargaHoraria,
    NivelDificuldade NivelDificuldade,
    decimal Valor,
    string CategoriaNome,
    List<TurmaCatalogoAlunoViewModel> Turmas
);

public sealed record MatriculaPainelAlunoViewModel(
    Guid Id,
    TurmaResumoAlunoViewModel Turma,
    CursoResumoAlunoViewModel Curso,
    ProfessorResumoAlunoViewModel Professor,
    DateTime DataMatricula,
    SituacaoMatricula Situacao,
    double? NotaFinal
)
{
    public int ProgressoCronologicoPercentual
    {
        get
        {
            if (DateTime.Today <= Turma.DataInicio.Date)
                return 0;

            if (DateTime.Today >= Turma.DataFim.Date)
                return 100;

            double duracao = (Turma.DataFim.Date - Turma.DataInicio.Date).TotalDays;
            double transcorrido = (DateTime.Today - Turma.DataInicio.Date).TotalDays;

            return duracao <= 0
                ? 0
                : (int)Math.Round(transcorrido / duracao * 100);
        }
    }
}

public record NotaAlunoViewModel(
    Guid Id,
    Guid MatriculaId,
    TipoNotaAluno TipoNota,
    string Descricao,
    double Valor,
    DateTime DataLancamento
);

public record PresencaAlunoViewModel(
    Guid Id,
    Guid MatriculaId,
    DateTime DataAula,
    bool Presente
);

public sealed class PainelAlunoViewModel
{
    public DetalhesAlunoViewModel Aluno { get; init; } = null!;
    public List<MatriculaPainelAlunoViewModel> Matriculas { get; init; } = [];
}

public sealed class CatalogoAlunoViewModel
{
    public List<CursoCatalogoAlunoViewModel> Cursos { get; init; } = [];
}

public sealed class DetalhesMatriculaAlunoViewModel
{
    public MatriculaPainelAlunoViewModel Matricula { get; init; } = null!;
    public List<NotaAlunoViewModel> Notas { get; init; } = [];
    public List<PresencaAlunoViewModel> Presencas { get; init; } = [];
    public ResumoProgressoModuloAlunoViewModel? ProgressoModulos
        { get; init; }

    public int FrequenciaPercentual
    {
        get
        {
            if (Presencas.Count == 0)
                return 0;

            int totalPresente = Presencas.Count(presenca => presenca.Presente);
            return (int)Math.Round(totalPresente * 100d / Presencas.Count);
        }
    }
}