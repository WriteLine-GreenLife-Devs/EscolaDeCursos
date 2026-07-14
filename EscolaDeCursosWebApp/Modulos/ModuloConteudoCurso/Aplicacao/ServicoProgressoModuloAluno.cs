using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using FluentResults;
using EntidadeModuloCurso = EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio.ModuloCurso;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Aplicacao;

public sealed class ServicoProgressoModuloAluno
    : ServicoBase<ProgressoModuloAluno>
{
    private readonly IRepositorioProgressoModuloAluno repositorioProgresso;
    private readonly IRepositorioModuloCurso repositorioModuloCurso;
    private readonly IRepositorioMatricula repositorioMatricula;
    private readonly IRepositorioTurma repositorioTurma;

    public ServicoProgressoModuloAluno(
        IRepositorioProgressoModuloAluno repositorioProgresso,
        IRepositorioModuloCurso repositorioModuloCurso,
        IRepositorioMatricula repositorioMatricula,
        IRepositorioTurma repositorioTurma)
    {
        this.repositorioProgresso = repositorioProgresso;
        this.repositorioModuloCurso = repositorioModuloCurso;
        this.repositorioMatricula = repositorioMatricula;
        this.repositorioTurma = repositorioTurma;
    }

    public Result AtualizarConclusaoDoAluno(
        AtualizarConclusaoModuloAlunoDto dto,
        Guid alunoId)
    {
        Matricula? matricula = repositorioMatricula.SelecionarPorId(
            dto.MatriculaId);

        if (matricula == null || matricula.AlunoId != alunoId)
        {
            return Falha(
                nameof(dto.MatriculaId),
                "Matrícula do aluno não encontrada.");
        }

        if (matricula.Situacao is SituacaoMatricula.Trancado or
            SituacaoMatricula.Reprovado)
        {
            return Falha(
                nameof(dto.MatriculaId),
                "A situação da matrícula não permite alterar o progresso.");
        }

        EntidadeModuloCurso? modulo = repositorioModuloCurso.SelecionarPorId(
            dto.ModuloCursoId);

        if (modulo == null || !modulo.Ativo)
            return Falha(nameof(dto.ModuloCursoId), "Módulo não encontrado.");

        Turma? turma = repositorioTurma.SelecionarPorId(matricula.TurmaId);

        if (turma == null || turma.cursoId != modulo.CursoId)
        {
            return Falha(
                nameof(dto.ModuloCursoId),
                "O módulo não pertence ao curso desta matrícula.");
        }

        ProgressoModuloAluno? progresso = repositorioProgresso
            .Filtrar(registro =>
                registro.MatriculaId == matricula.Id &&
                registro.ModuloCursoId == modulo.Id)
            .SingleOrDefault();

        if (progresso == null)
        {
            if (!dto.Concluido)
                return Result.Ok();

            progresso = new ProgressoModuloAluno(
                matricula.Id,
                modulo.Id);

            progresso.MarcarComoConcluido();

            Result resultadoValidacao = ValidarEntidade(progresso);

            if (resultadoValidacao.IsFailed)
                return resultadoValidacao;

            repositorioProgresso.Cadastrar(progresso);

            return Result.Ok();
        }

        if (progresso.Concluido == dto.Concluido)
            return Result.Ok();

        if (dto.Concluido)
            progresso.MarcarComoConcluido();
        else
            progresso.MarcarComoPendente();

        Result validacaoProgresso = ValidarEntidade(progresso);

        if (validacaoProgresso.IsFailed)
            return validacaoProgresso;

        return repositorioProgresso.Editar(progresso.Id, progresso)
            ? Result.Ok()
            : Falha(string.Empty, "Não foi possível atualizar o progresso.");
    }

    public ResumoProgressoModuloAlunoDto? SelecionarProgressoDoAluno(
        Guid matriculaId,
        Guid alunoId)
    {
        Matricula? matricula = repositorioMatricula.SelecionarPorId(
            matriculaId);

        if (matricula == null || matricula.AlunoId != alunoId)
            return null;

        return SelecionarProgressoDaMatricula(matriculaId);
    }

    public ResumoProgressoModuloAlunoDto? SelecionarProgressoDaMatricula(
        Guid matriculaId)
    {
        Matricula? matricula = repositorioMatricula.SelecionarPorId(
            matriculaId);

        if (matricula == null)
            return null;

        Turma? turma = repositorioTurma.SelecionarPorId(matricula.TurmaId);

        if (turma == null)
            return null;

        List<EntidadeModuloCurso> modulos = repositorioModuloCurso
            .Filtrar(modulo =>
                modulo.CursoId == turma.cursoId &&
                modulo.Ativo)
            .OrderBy(modulo => modulo.Ordem)
            .ToList();

        List<ProgressoModuloAluno> progressos = repositorioProgresso
            .Filtrar(progresso =>
                progresso.MatriculaId == matricula.Id);

        List<ModuloProgressoAlunoDto> modulosDto = modulos
            .Select(modulo =>
            {
                ProgressoModuloAluno? progresso = progressos
                    .SingleOrDefault(registro =>
                        registro.ModuloCursoId == modulo.Id);

                return new ModuloProgressoAlunoDto(
                    modulo.Id,
                    modulo.Titulo,
                    modulo.Descricao,
                    modulo.DuracaoMinutos,
                    modulo.Ordem,
                    progresso?.Concluido ?? false,
                    progresso?.DataConclusao);
            })
            .ToList();

        long duracaoTotal = modulos.Sum(modulo =>
            (long)modulo.DuracaoMinutos);

        long duracaoConcluida = modulosDto
            .Where(modulo => modulo.Concluido)
            .Sum(modulo => (long)modulo.DuracaoMinutos);

        int percentualConclusao = duracaoTotal == 0
            ? 0
            : (int)Math.Round(
                duracaoConcluida * 100d / duracaoTotal);

        return new ResumoProgressoModuloAlunoDto(
            matricula.Id,
            modulosDto.Count,
            modulosDto.Count(modulo => modulo.Concluido),
            duracaoTotal,
            duracaoConcluida,
            percentualConclusao,
            modulosDto);
    }
}