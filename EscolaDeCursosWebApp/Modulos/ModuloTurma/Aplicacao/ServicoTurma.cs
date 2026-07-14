using System;
using System.Linq;
using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloTurma.Aplicacao;

public sealed class ServicoTurma : ServicoBase<Turma>
{
    private readonly IRepositorioTurma repositorioTurma;
    private readonly IRepositorioCurso repositorioCurso;
    private readonly IRepositorioUsuario repositorioUsuario;
    private readonly IRepositorioMatricula repositorioMatricula;

    public ServicoTurma(
        IRepositorioTurma repositorioTurma,
        IRepositorioCurso repositorioCurso,
        IRepositorioUsuario repositorioUsuario,
        IRepositorioMatricula repositorioMatricula
    )
    {
        this.repositorioTurma = repositorioTurma;
        this.repositorioCurso = repositorioCurso;
        this.repositorioUsuario = repositorioUsuario;
        this.repositorioMatricula = repositorioMatricula;
    }

    public Result CadastrarTurma(CadastrarTurmaDto cadastrarTurmaDto)
    {
        if (repositorioCurso.SelecionarPorId(cadastrarTurmaDto.cursoId) == null)
            return Falha(nameof(cadastrarTurmaDto.cursoId), "Curso não encontrado.");

        if (repositorioUsuario.SelecionarPorId(cadastrarTurmaDto.instrutorId) is not Usuario instrutor ||
            instrutor.tipoUsuario != TipoUsuario.Professor ||
            !instrutor.ativo)
            return Falha(nameof(cadastrarTurmaDto.instrutorId), "Instrutor não encontrado ou inválido.");

        var turma = new Turma
        {
            nome = cadastrarTurmaDto.nome,
            dataInicio = cadastrarTurmaDto.dataInicio,
            dataFim = cadastrarTurmaDto.dataFim,
            vagasMaximas = cadastrarTurmaDto.vagasMaximas,
            HorarioTurno = cadastrarTurmaDto.HorarioTurno,
            status = cadastrarTurmaDto.status,
            cursoId = cadastrarTurmaDto.cursoId,
            instrutorId = cadastrarTurmaDto.instrutorId
        };

        Result resultadoValidacao = ValidarEntidade(turma);
        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioTurma.Cadastrar(turma);
        return Result.Ok();
    }

    public Result EditarTurma(Guid id, EditarTurmaDto editarTurmaDto)
    {
        var turmaExistente = repositorioTurma.SelecionarPorId(id);
        if (turmaExistente == null)
            return Falha("Id", "Turma não encontrada.");

        if (repositorioCurso.SelecionarPorId(editarTurmaDto.cursoId) == null)
            return Falha(nameof(editarTurmaDto.cursoId), "Curso não encontrado.");

        if (repositorioUsuario.SelecionarPorId(editarTurmaDto.instrutorId) is not Usuario instrutor ||
            instrutor.tipoUsuario != TipoUsuario.Professor ||
            (!instrutor.ativo && turmaExistente.instrutorId != instrutor.Id))
            return Falha(nameof(editarTurmaDto.instrutorId), "Instrutor não encontrado ou inválido.");

        var turmaAtualizada = new Turma
        {
            nome = editarTurmaDto.nome,
            dataInicio = editarTurmaDto.dataInicio,
            dataFim = editarTurmaDto.dataFim,
            vagasMaximas = editarTurmaDto.vagasMaximas,
            HorarioTurno = editarTurmaDto.HorarioTurno,
            status = editarTurmaDto.status,
            cursoId = editarTurmaDto.cursoId,
            instrutorId = editarTurmaDto.instrutorId
        };

        Result resultadoValidacao = ValidarEntidade(turmaAtualizada);
        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        if (!repositorioTurma.Editar(id, turmaAtualizada))
            return Falha(string.Empty, "Falha ao atualizar a turma.");

        return Result.Ok();
    }

    public bool ExcluirTurma(Guid id)
    {
        return repositorioTurma.Excluir(id);
    }

    public List<TurmaProfessorDto> SelecionarPorProfessor(Guid professorId)
    {
        return repositorioTurma
            .Filtrar(turma => turma.instrutorId == professorId)
            .Select(turma =>
            {
                Curso? curso = repositorioCurso.SelecionarPorId(turma.cursoId);

                int totalAlunos = repositorioMatricula
                    .Filtrar(matricula =>
                        matricula.TurmaId == turma.Id &&
                        matricula.Situacao == SituacaoMatricula.Cursando)
                    .Count;

                return new TurmaProfessorDto(
                    turma.Id,
                    turma.cursoId,
                    turma.nome,
                    curso?.nome ?? "Curso não encontrado",
                    turma.dataInicio,
                    turma.dataFim,
                    turma.HorarioTurno,
                    turma.status,
                    totalAlunos);
            })
            .OrderBy(turma => turma.DataInicio)
            .ToList();
    }
}