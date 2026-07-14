using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Apresentacao;

public record DetalhesAlunoViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    bool Ativo
);

public record MatriculaPainelAlunoViewModel(
    Guid Id,
    Guid TurmaId,
    string TurmaNome,
    string CursoNome,
    DateTime DataMatricula,
    SituacaoMatricula Situacao
);

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

public sealed class DetalhesMatriculaAlunoViewModel
{
    public MatriculaPainelAlunoViewModel Matricula { get; init; } = null!;
    public List<NotaAlunoViewModel> Notas { get; init; } = [];
    public List<PresencaAlunoViewModel> Presencas { get; init; } = [];
}