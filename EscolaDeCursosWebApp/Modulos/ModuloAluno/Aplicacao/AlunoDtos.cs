using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;

public record CadastrarAlunoDto(
    string Nome,
    string Email,
    string Senha,
    string Telefone
);

public record ListarAlunosDto(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    bool Ativo
);

public record DetalhesAlunoDto(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    bool Ativo
);

public record TurmaResumoAlunoDto(
    Guid Id,
    string Nome,
    DateTime DataInicio,
    DateTime DataFim,
    string HorarioTurno,
    StatusTurma Status
);

public record CursoResumoAlunoDto(
    Guid Id,
    string Nome,
    string Descricao,
    int CargaHoraria,
    NivelDificuldade NivelDificuldade
);

public record ProfessorResumoAlunoDto(
    Guid Id,
    string Nome,
    string Email,
    string Bio,
    string Especialidades
);

public record MatriculaPainelAlunoDto(
    Guid Id,
    TurmaResumoAlunoDto Turma,
    CursoResumoAlunoDto Curso,
    ProfessorResumoAlunoDto Professor,
    DateTime DataMatricula,
    SituacaoMatricula Situacao,
    double? NotaFinal
);

public record CadastrarNotaAlunoDto(
    Guid MatriculaId,
    TipoNotaAluno TipoNota,
    string Descricao,
    double Valor,
    DateTime DataLancamento
);

public record EditarNotaAlunoDto(
    Guid Id,
    TipoNotaAluno TipoNota,
    string Descricao,
    double Valor,
    DateTime DataLancamento
);

public record NotaAlunoDto(
    Guid Id,
    Guid AlunoId,
    Guid MatriculaId,
    TipoNotaAluno TipoNota,
    string Descricao,
    double Valor,
    DateTime DataLancamento
);

public record RegistrarPresencaAlunoDto(
    Guid MatriculaId,
    DateTime DataAula,
    bool Presente
);

public record EditarPresencaAlunoDto(
    Guid Id,
    DateTime DataAula,
    bool Presente
);

public record PresencaAlunoDto(
    Guid Id,
    Guid AlunoId,
    Guid MatriculaId,
    DateTime DataAula,
    bool Presente
);