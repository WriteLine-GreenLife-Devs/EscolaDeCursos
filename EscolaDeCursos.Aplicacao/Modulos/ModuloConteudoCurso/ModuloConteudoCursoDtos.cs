namespace EscolaDeCursos.Aplicacao.Modulos.ModuloConteudoCurso;

public sealed record CadastrarModuloCursoDto(
    Guid CursoId,
    string Titulo,
    string Descricao,
    int DuracaoMinutos,
    int Ordem
);

public sealed record EditarModuloCursoDto(
    Guid Id,
    string Titulo,
    string Descricao,
    int DuracaoMinutos,
    int Ordem
);

public sealed record ModuloCursoDto(
    Guid Id,
    Guid CursoId,
    string Titulo,
    string Descricao,
    int DuracaoMinutos,
    int Ordem,
    bool Ativo
);

public sealed record AtualizarConclusaoModuloAlunoDto(
    Guid MatriculaId,
    Guid ModuloCursoId,
    bool Concluido
);

public sealed record ModuloProgressoAlunoDto(
    Guid ModuloCursoId,
    string Titulo,
    string Descricao,
    int DuracaoMinutos,
    int Ordem,
    bool Concluido,
    DateTime? DataConclusao
);

public sealed record ResumoProgressoModuloAlunoDto(
    Guid MatriculaId,
    int TotalModulos,
    int ModulosConcluidos,
    long DuracaoTotalMinutos,
    long DuracaoConcluidaMinutos,
    int PercentualConclusao,
    List<ModuloProgressoAlunoDto> Modulos
);