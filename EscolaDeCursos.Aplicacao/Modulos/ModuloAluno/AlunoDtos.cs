using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;

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

public record TurmaCatalogoAlunoDto(
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

public record CursoCatalogoAlunoDto(
    Guid Id,
    string Nome,
    string Descricao,
    int CargaHoraria,
    NivelDificuldade NivelDificuldade,
    decimal Valor,
    string CategoriaNome,
    List<TurmaCatalogoAlunoDto> Turmas
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

public record SalvarNotasAlunoDto(
    Guid MatriculaId,
    double? Nota1,
    double? Nota2,
    double? Nota3,
    double? Recuperacao
);

public record FichaNotasAlunoDto(
    Guid MatriculaId,
    double? Nota1,
    double? Nota2,
    double? Nota3,
    double? Recuperacao,
    double? NotaFinal,
    SituacaoMatricula Situacao
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

public record PresencaChamadaAlunoDto(
    Guid MatriculaId,
    bool Presente
);

public record SalvarChamadaAlunoDto(
    Guid TurmaId,
    DateTime DataAula,
    List<PresencaChamadaAlunoDto> Alunos
);