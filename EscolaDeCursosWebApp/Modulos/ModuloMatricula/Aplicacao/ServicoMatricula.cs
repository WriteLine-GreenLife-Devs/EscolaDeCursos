using System;
using System.Linq;
using System.Collections.Generic;
using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;

public sealed class ServicoMatricula : ServicoBase<Matricula>
{
    private readonly IRepositorioMatricula repositorioMatricula;
    private readonly IRepositorioTurma repositorioTurma;
    private readonly IRepositorioUsuario repositorioUsuario;

    public ServicoMatricula(
        IRepositorioMatricula repositorioMatricula,
        IRepositorioTurma repositorioTurma,
        IRepositorioUsuario repositorioUsuario
    )
    {
        this.repositorioMatricula = repositorioMatricula;
        this.repositorioTurma = repositorioTurma;
        this.repositorioUsuario = repositorioUsuario;
    }

    public Result CadastrarMatricula(CadastrarMatriculaDto dto)
    {
        if (dto == null)
            return Falha(string.Empty, "Dados de matrícula inválidos.");

        var turma = repositorioTurma.SelecionarPorId(dto.TurmaId);
        if (turma == null)
            return Falha(nameof(dto.TurmaId), "Turma não encontrada.");

        var aluno = repositorioUsuario.SelecionarPorId(dto.AlunoId);
        if (aluno == null || aluno.tipoUsuario != TipoUsuario.Aluno)
            return Falha(nameof(dto.AlunoId), "Aluno não encontrado ou inválido.");

        var matriculaExistente = repositorioMatricula.SelecionarTodos()
            .Any(m => m.TurmaId == dto.TurmaId && m.AlunoId == dto.AlunoId);

        if (matriculaExistente)
            return Falha(nameof(dto.AlunoId), "O aluno já está matriculado nesta turma.");

        int vagasOcupadas = repositorioMatricula.SelecionarTodos()
            .Count(m => m.TurmaId == dto.TurmaId && m.Situacao == SituacaoMatricula.Cursando);

        if (vagasOcupadas >= turma.vagasMaximas)
            return Falha(string.Empty, "Não há vagas disponíveis para esta turma.");

        var matricula = new Matricula(dto.AlunoId, dto.TurmaId, DateTime.Now, SituacaoMatricula.Cursando);
        var validacao = ValidarEntidade(matricula);

        if (validacao.IsFailed)
            return validacao;

        repositorioMatricula.Cadastrar(matricula);
        return Result.Ok();
    }

    public Result AlterarSituacaoMatricula(Guid matriculaId, SituacaoMatricula novaSituacao)
    {
        var matricula = repositorioMatricula.SelecionarPorId(matriculaId);
        if (matricula == null)
            return Falha(nameof(matriculaId), "Matrícula não encontrada.");

        if (matricula.Situacao == novaSituacao)
            return Falha(string.Empty, "A situação já está definida dessa forma.");

        if (matricula.Situacao == SituacaoMatricula.Cursando && novaSituacao != SituacaoMatricula.Trancado)
            return Falha(string.Empty, "Apenas matrículas em curso podem ser trancadas.");

        if (matricula.Situacao == SituacaoMatricula.Trancado && novaSituacao != SituacaoMatricula.Cursando)
            return Falha(string.Empty, "Apenas matrículas trancadas podem ser reativadas.");

        if (novaSituacao == SituacaoMatricula.Cursando)
        {
            var turma = repositorioTurma.SelecionarPorId(matricula.TurmaId);
            if (turma == null)
                return Falha(nameof(matricula.TurmaId), "Turma não encontrada.");

            int vagasOcupadas = repositorioMatricula.SelecionarTodos()
                .Count(m => m.TurmaId == matricula.TurmaId && m.Situacao == SituacaoMatricula.Cursando);

            if (vagasOcupadas >= turma.vagasMaximas)
                return Falha(string.Empty, "Não há vagas disponíveis para reativar essa matrícula.");
        }

        var matriculaAtualizada = new Matricula(matricula.AlunoId, matricula.TurmaId, matricula.DataMatricula, novaSituacao);

        if (!repositorioMatricula.Editar(matriculaId, matriculaAtualizada))
            return Falha(string.Empty, "Falha ao atualizar a situação da matrícula.");

        return Result.Ok();
    }

    public List<Matricula> SelecionarMatriculasPorTurma(Guid turmaId)
    {
        return repositorioMatricula.SelecionarTodos().Where(m => m.TurmaId == turmaId).ToList();
    }

    public bool ExcluirMatricula(Guid id)
    {
        return repositorioMatricula.Excluir(id);
    }
}
