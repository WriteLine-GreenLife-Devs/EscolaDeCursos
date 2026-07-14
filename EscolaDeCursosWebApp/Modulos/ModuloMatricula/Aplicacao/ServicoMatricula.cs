using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;

public sealed class ServicoMatricula : ServicoBase<Matricula>
{
    private readonly IRepositorioMatricula repositorioMatricula;
    private readonly IRepositorioTurma repositorioTurma;
    private readonly IRepositorioCurso repositorioCurso;
    private readonly IRepositorioUsuario repositorioUsuario;

    public ServicoMatricula(
        IRepositorioMatricula repositorioMatricula,
        IRepositorioTurma repositorioTurma,
        IRepositorioCurso repositorioCurso,
        IRepositorioUsuario repositorioUsuario)
    {
        this.repositorioMatricula = repositorioMatricula;
        this.repositorioTurma = repositorioTurma;
        this.repositorioCurso = repositorioCurso;
        this.repositorioUsuario = repositorioUsuario;
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    public Result CadastrarMatricula(CadastrarMatriculaDto dto)
    {
        var turma = repositorioTurma.SelecionarPorId(dto.TurmaId);
        if (turma == null)
            return Falha(nameof(dto.TurmaId), "Turma não encontrada.");

        Curso? curso = repositorioCurso.SelecionarPorId(turma.cursoId);

        if (curso == null || curso.status != StatusCurso.Ativo)
            return Falha(nameof(dto.TurmaId), "O curso não está disponível para matrícula.");

        if (turma.status != StatusTurma.InscricoesAbertas ||
            turma.dataFim.Date < DateTime.Today)
        {
            return Falha(nameof(dto.TurmaId), "As inscrições desta turma não estão abertas.");
        }

        var aluno = repositorioUsuario.SelecionarPorId(dto.AlunoId);
        if (aluno == null || aluno.tipoUsuario != TipoUsuario.Aluno || !aluno.ativo)
            return Falha(nameof(dto.AlunoId), "Aluno não encontrado ou inválido.");

        var matricula = new Matricula(dto.AlunoId, dto.TurmaId, DateTime.Now, SituacaoMatricula.Cursando);

        var erros = matricula.Validar();
        if (erros.Count > 0)
            return Falha(string.Empty, erros.First());

        ResultadoCadastroMatricula resultadoCadastro = repositorioMatricula
            .CadastrarComControleDeVagas(matricula, turma.vagasMaximas);

        return resultadoCadastro switch
        {
            ResultadoCadastroMatricula.Sucesso => Result.Ok(),
            ResultadoCadastroMatricula.MatriculaExistente =>
                Falha(string.Empty, "Aluno já está matriculado nesta turma."),
            ResultadoCadastroMatricula.SemVagas =>
                Falha(string.Empty, "Não há vagas disponíveis nesta turma."),
            _ => Falha(string.Empty, "Não foi possível realizar a matrícula.")
        };
    }

    public Result AlterarSituacaoMatricula(AlterarSituacaoMatriculaDto dto)
    {
        var matricula = repositorioMatricula.SelecionarPorId(dto.MatriculaId);
        if (matricula == null)
            return Falha(nameof(dto.MatriculaId), "Matrícula não encontrada.");

        if (matricula.Situacao == SituacaoMatricula.Trancado)
            return Falha(string.Empty, "Matrícula trancada não pode ser alterada.");

        // Permitimos apenas trancar a matrícula através desta ação (conforme regra atual)
        if (dto.NovaSituacao != SituacaoMatricula.Trancado)
            return Falha(nameof(dto.NovaSituacao), "Só é permitido alterar a situação para 'Trancado' via esta ação.");

        matricula.Situacao = SituacaoMatricula.Trancado;

        if (!repositorioMatricula.Editar(dto.MatriculaId, matricula))
            return Falha(string.Empty, "Falha ao atualizar situação da matrícula.");

        return Result.Ok();
    }

    public TurmaDetalheDto? ObterDetalhesTurma(Guid turmaId)
    {
        var turma = repositorioTurma.SelecionarPorId(turmaId);
        if (turma == null) return null;

        var instrutor = repositorioUsuario.SelecionarPorId(turma.instrutorId);

        var matriculas = repositorioMatricula.Filtrar(m => m.TurmaId == turmaId)
            .Select(m => new MatriculaAlunoDto
            {
                MatriculaId = m.Id,
                AlunoId = m.AlunoId,
                AlunoNome = (repositorioUsuario.SelecionarPorId(m.AlunoId)?.nome) ?? string.Empty,
                Situacao = m.Situacao,
                DataMatricula = m.DataMatricula
            })
            .OrderBy(m => m.DataMatricula)
            .ToList();

        return new TurmaDetalheDto
        {
            TurmaId = turma.Id,
            TurmaNome = turma.nome,
            InstrutorId = turma.instrutorId,
            InstrutorNome = instrutor?.nome ?? string.Empty,
            VagasMaximas = turma.vagasMaximas,
            Matriculas = matriculas
        };
    }

    public TurmaDetalheDto? ObterDetalhesTurmaDoProfessor(
        Guid turmaId,
        Guid professorId)
    {
        Turma? turma = repositorioTurma.SelecionarPorId(turmaId);

        if (turma == null || turma.instrutorId != professorId)
            return null;

        return ObterDetalhesTurma(turmaId);
    }

    public FichaNotasDto? ObterFichaNotas(Guid matriculaId)
    {
        var matricula = repositorioMatricula.SelecionarPorId(matriculaId);
        if (matricula == null) return null;

        var aluno = repositorioUsuario.SelecionarPorId(matricula.AlunoId);

        return new FichaNotasDto
        {
            MatriculaId = matricula.Id,
            AlunoId = matricula.AlunoId,
            AlunoNome = aluno?.nome ?? string.Empty,
            Nota1 = matricula.Nota1,
            Nota2 = matricula.Nota2,
            Nota3 = matricula.Nota3,
            Recuperacao = matricula.Recuperacao,
            NotaFinal = matricula.NotaFinal,
            Situacao = matricula.Situacao
        };
    }

    public Result AtualizarNotas(AtualizarNotasDto dto)
    {
        var matricula = repositorioMatricula.SelecionarPorId(dto.MatriculaId);
        if (matricula == null)
            return Falha(nameof(dto.MatriculaId), "Matrícula não encontrada.");

        if (matricula.Situacao != SituacaoMatricula.Cursando)
            return Falha(string.Empty, "Somente matrículas com situação 'Cursando' podem ter notas alteradas.");

        try
        {
            matricula.AtualizarNotas(dto.Nota1, dto.Nota2, dto.Nota3, dto.Recuperacao);
        }
        catch (ArgumentException ex)
        {
            return Falha(string.Empty, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Falha(string.Empty, ex.Message);
        }

        if (!repositorioMatricula.Editar(dto.MatriculaId, matricula))
            return Falha(string.Empty, "Falha ao salvar notas.");

        return Result.Ok();
    }
}