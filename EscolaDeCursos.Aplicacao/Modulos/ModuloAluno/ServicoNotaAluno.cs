using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;

public sealed class ServicoNotaAluno : ServicoBase<NotaAluno>
{
    private readonly IRepositorioAluno repositorioAluno;
    private readonly IRepositorioNotaAluno repositorioNotaAluno;
    private readonly IRepositorioMatricula repositorioMatricula;
    private readonly IRepositorioTurma repositorioTurma;

    public ServicoNotaAluno(
        IRepositorioAluno repositorioAluno,
        IRepositorioNotaAluno repositorioNotaAluno,
        IRepositorioMatricula repositorioMatricula,
        IRepositorioTurma repositorioTurma)
    {
        this.repositorioAluno = repositorioAluno;
        this.repositorioNotaAluno = repositorioNotaAluno;
        this.repositorioMatricula = repositorioMatricula;
        this.repositorioTurma = repositorioTurma;
    }

    public Result Cadastrar(CadastrarNotaAlunoDto dto)
    {
        Matricula? matricula = SelecionarMatriculaDoAluno(dto.MatriculaId);

        if (matricula == null)
            return Falha(nameof(dto.MatriculaId), "Matrícula de aluno não encontrada.");

        if (matricula.Situacao == SituacaoMatricula.Trancado)
            return Falha(nameof(dto.MatriculaId), "Não é possível lançar nota em uma matrícula trancada.");

        bool tipoJaRegistrado = repositorioNotaAluno.Filtrar(nota =>
            nota.MatriculaId == dto.MatriculaId &&
            nota.TipoNota == dto.TipoNota).Any();

        if (tipoJaRegistrado)
            return Falha(nameof(dto.TipoNota), "Esta nota já foi lançada para a matrícula.");

        var nota = new NotaAluno(
            matricula.AlunoId,
            matricula.Id,
            dto.TipoNota,
            dto.Descricao,
            dto.Valor,
            dto.DataLancamento
        );

        Result resultadoValidacao = ValidarNota(nota, matricula);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        List<NotaAluno> notas = SelecionarNotas(matricula.Id);
        notas.Add(nota);

        Result resultadoCalculo = SincronizarResumoMatricula(matricula, notas);

        if (resultadoCalculo.IsFailed)
            return resultadoCalculo;

        repositorioNotaAluno.Cadastrar(nota);

        return Result.Ok();
    }

    public Result Editar(EditarNotaAlunoDto dto)
    {
        NotaAluno? notaExistente = repositorioNotaAluno.SelecionarPorId(dto.Id);

        if (notaExistente == null)
            return Falha(nameof(dto.Id), "Nota não encontrada.");

        Matricula? matricula = SelecionarMatriculaDoAluno(
            notaExistente.MatriculaId);

        if (matricula == null)
            return Falha(nameof(dto.Id), "Matrícula de aluno não encontrada.");

        if (matricula.Situacao == SituacaoMatricula.Trancado)
            return Falha(nameof(dto.Id), "Não é possível editar nota de uma matrícula trancada.");

        bool tipoJaRegistrado = repositorioNotaAluno.Filtrar(nota =>
            nota.Id != dto.Id &&
            nota.MatriculaId == matricula.Id &&
            nota.TipoNota == dto.TipoNota).Any();

        if (tipoJaRegistrado)
            return Falha(nameof(dto.TipoNota), "Esta nota já foi lançada para a matrícula.");

        var notaAtualizada = new NotaAluno(
            notaExistente.AlunoId,
            notaExistente.MatriculaId,
            dto.TipoNota,
            dto.Descricao,
            dto.Valor,
            dto.DataLancamento
        );

        Result resultadoValidacao = ValidarNota(notaAtualizada, matricula);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        List<NotaAluno> notas = SelecionarNotas(matricula.Id)
            .Where(nota => nota.Id != dto.Id)
            .ToList();

        notas.Add(notaAtualizada);

        Result resultadoCalculo = SincronizarResumoMatricula(matricula, notas);

        if (resultadoCalculo.IsFailed)
            return resultadoCalculo;

        return repositorioNotaAluno.Editar(dto.Id, notaAtualizada)
            ? Result.Ok()
            : Falha(string.Empty, "Não foi possível editar a nota.");
    }

    public Result Excluir(Guid notaId)
    {
        NotaAluno? nota = repositorioNotaAluno.SelecionarPorId(notaId);

        if (nota == null)
            return Falha(nameof(notaId), "Nota não encontrada.");

        Matricula? matricula = SelecionarMatriculaDoAluno(nota.MatriculaId);

        if (matricula == null)
            return Falha(nameof(notaId), "Matrícula de aluno não encontrada.");

        if (matricula.Situacao == SituacaoMatricula.Trancado)
            return Falha(nameof(notaId), "Não é possível excluir nota de uma matrícula trancada.");

        List<NotaAluno> notasRestantes = SelecionarNotas(matricula.Id)
            .Where(notaRegistrada => notaRegistrada.Id != notaId)
            .ToList();

        Result resultadoCalculo =
            SincronizarResumoMatricula(matricula, notasRestantes);

        if (resultadoCalculo.IsFailed)
            return resultadoCalculo;

        return repositorioNotaAluno.Excluir(notaId)
            ? Result.Ok()
            : Falha(string.Empty, "Não foi possível excluir a nota.");
    }

    public List<NotaAlunoDto> SelecionarPorMatricula(Guid matriculaId)
    {
        return SelecionarNotas(matriculaId)
            .OrderBy(nota => nota.TipoNota)
            .Select(MapearNota)
            .ToList();
    }

    public FichaNotasAlunoDto? SelecionarFicha(Guid matriculaId)
    {
        Matricula? matricula = SelecionarMatriculaDoAluno(matriculaId);

        if (matricula == null)
            return null;

        List<NotaAluno> notas = SelecionarNotas(matriculaId);

        return new FichaNotasAlunoDto(
            matricula.Id,
            SelecionarValorNota(notas, TipoNotaAluno.Nota1) ?? matricula.Nota1,
            SelecionarValorNota(notas, TipoNotaAluno.Nota2) ?? matricula.Nota2,
            SelecionarValorNota(notas, TipoNotaAluno.Nota3) ?? matricula.Nota3,
            SelecionarValorNota(notas, TipoNotaAluno.Recuperacao) ?? matricula.Recuperacao,
            matricula.NotaFinal,
            matricula.Situacao);
    }

    public Result SalvarNotasDoProfessor(
        SalvarNotasAlunoDto dto,
        Guid professorId)
    {
        Matricula? matricula = SelecionarMatriculaDoAluno(dto.MatriculaId);

        if (matricula == null)
            return Falha(nameof(dto.MatriculaId), "Matrícula de aluno não encontrada.");

        Turma? turma = repositorioTurma.SelecionarPorId(matricula.TurmaId);

        if (turma == null || turma.instrutorId != professorId)
            return Falha(nameof(dto.MatriculaId), "A matrícula não pertence a uma turma deste professor.");

        return SalvarNotas(dto, matricula);
    }

    public Result SalvarNotasDoAdministrador(SalvarNotasAlunoDto dto)
    {
        Matricula? matricula = SelecionarMatriculaDoAluno(dto.MatriculaId);

        if (matricula == null)
            return Falha(nameof(dto.MatriculaId), "Matrícula de aluno não encontrada.");

        return SalvarNotas(dto, matricula);
    }

    private Result SalvarNotas(
        SalvarNotasAlunoDto dto,
        Matricula matricula)
    {
        if (matricula.Situacao == SituacaoMatricula.Trancado)
            return Falha(nameof(dto.MatriculaId), "Não é possível editar notas de uma matrícula trancada.");

        List<NotaAluno> notasRegistradas = SelecionarNotas(matricula.Id);
        List<NotaAluno> notasNovas = [];
        List<NotaAluno> notasRemovidas = [];

        (TipoNotaAluno Tipo, string Descricao, double? Valor)[] valores =
        [
            (TipoNotaAluno.Nota1, "Nota 1", dto.Nota1),
            (TipoNotaAluno.Nota2, "Nota 2", dto.Nota2),
            (TipoNotaAluno.Nota3, "Nota 3", dto.Nota3),
            (TipoNotaAluno.Recuperacao, "Recuperação", dto.Recuperacao)
        ];

        foreach (var valorNota in valores)
        {
            NotaAluno? notaRegistrada = notasRegistradas.SingleOrDefault(nota =>
                nota.TipoNota == valorNota.Tipo);

            if (!valorNota.Valor.HasValue)
            {
                if (notaRegistrada != null)
                    notasRemovidas.Add(notaRegistrada);

                continue;
            }

            var notaAtualizada = new NotaAluno(
                matricula.AlunoId,
                matricula.Id,
                valorNota.Tipo,
                valorNota.Descricao,
                valorNota.Valor.Value,
                notaRegistrada?.DataLancamento ?? DateTime.Today);

            Result resultadoValidacao = ValidarNota(
                notaAtualizada,
                matricula);

            if (resultadoValidacao.IsFailed)
                return resultadoValidacao;

            if (notaRegistrada == null)
                notasNovas.Add(notaAtualizada);
            else
                notaRegistrada.Atualizar(notaAtualizada);
        }

        List<NotaAluno> notasFinais = notasRegistradas
            .Except(notasRemovidas)
            .Concat(notasNovas)
            .ToList();

        Result resultadoCalculo = SincronizarResumoMatricula(
            matricula,
            notasFinais);

        if (resultadoCalculo.IsFailed)
            return resultadoCalculo;

        repositorioNotaAluno.SalvarAlteracoes(
            notasNovas,
            notasRemovidas);

        return Result.Ok();
    }

    private Matricula? SelecionarMatriculaDoAluno(Guid matriculaId)
    {
        Matricula? matricula = repositorioMatricula.SelecionarPorId(matriculaId);

        if (matricula == null)
            return null;

        return repositorioAluno.SelecionarPorId(matricula.AlunoId) == null
            ? null
            : matricula;
    }

    private List<NotaAluno> SelecionarNotas(Guid matriculaId)
    {
        return repositorioNotaAluno.Filtrar(nota =>
            nota.MatriculaId == matriculaId);
    }

    private static Result ValidarNota(
        NotaAluno nota,
        Matricula matricula)
    {
        Result resultadoValidacao = ValidarEntidade(nota);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        if (nota.DataLancamento.Date < matricula.DataMatricula.Date)
            return Falha(nameof(nota.DataLancamento), "A nota não pode ser anterior à matrícula.");

        return Result.Ok();
    }

    private static Result SincronizarResumoMatricula(
        Matricula matricula,
        List<NotaAluno> notas)
    {
        double? nota1 = SelecionarValorNota(notas, TipoNotaAluno.Nota1);
        double? nota2 = SelecionarValorNota(notas, TipoNotaAluno.Nota2);
        double? nota3 = SelecionarValorNota(notas, TipoNotaAluno.Nota3);
        double? recuperacao = SelecionarValorNota(
            notas,
            TipoNotaAluno.Recuperacao);

        matricula.Situacao = SituacaoMatricula.Cursando;
        matricula.NotaFinal = null;

        try
        {
            matricula.AtualizarNotas(
                nota1,
                nota2,
                nota3,
                recuperacao);

            return Result.Ok();
        }
        catch (Exception excecao)
        {
            return Falha(string.Empty, excecao.Message);
        }
    }

    private static double? SelecionarValorNota(
        List<NotaAluno> notas,
        TipoNotaAluno tipoNota)
    {
        return notas.SingleOrDefault(nota =>
            nota.TipoNota == tipoNota)?.Valor;
    }

    private static NotaAlunoDto MapearNota(NotaAluno nota)
    {
        return new NotaAlunoDto(
            nota.Id,
            nota.AlunoId,
            nota.MatriculaId,
            nota.TipoNota,
            nota.Descricao,
            nota.Valor,
            nota.DataLancamento
        );
    }
}