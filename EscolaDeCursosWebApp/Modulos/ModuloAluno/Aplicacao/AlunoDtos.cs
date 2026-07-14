using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;

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