using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using FluentResults;
using EntidadeModuloCurso = EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio.ModuloCurso;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Aplicacao;

public sealed class ServicoModuloCurso : ServicoBase<EntidadeModuloCurso>
{
    private readonly IRepositorioModuloCurso repositorioModuloCurso;
    private readonly IRepositorioCurso repositorioCurso;

    public ServicoModuloCurso(
        IRepositorioModuloCurso repositorioModuloCurso,
        IRepositorioCurso repositorioCurso)
    {
        this.repositorioModuloCurso = repositorioModuloCurso;
        this.repositorioCurso = repositorioCurso;
    }

    public Result Cadastrar(CadastrarModuloCursoDto dto)
    {
        Curso? curso = repositorioCurso.SelecionarPorId(dto.CursoId);

        if (curso == null)
            return Falha(nameof(dto.CursoId), "Curso não encontrado.");

        var modulo = new EntidadeModuloCurso(
            dto.CursoId,
            dto.Titulo,
            dto.Descricao,
            dto.DuracaoMinutos,
            dto.Ordem);

        Result resultadoValidacao = ValidarEntidade(modulo);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        Result resultadoOrdem = ValidarOrdemUnica(modulo);

        if (resultadoOrdem.IsFailed)
            return resultadoOrdem;

        Result resultadoDuracao = ValidarDuracaoDoCurso(
            curso,
            modulo);

        if (resultadoDuracao.IsFailed)
            return resultadoDuracao;

        repositorioModuloCurso.Cadastrar(modulo);

        return Result.Ok();
    }

    public Result Editar(EditarModuloCursoDto dto)
    {
        EntidadeModuloCurso? moduloExistente = repositorioModuloCurso
            .SelecionarPorId(dto.Id);

        if (moduloExistente == null)
            return Falha(nameof(dto.Id), "Módulo não encontrado.");

        Curso? curso = repositorioCurso.SelecionarPorId(
            moduloExistente.CursoId);

        if (curso == null)
            return Falha(string.Empty, "Curso do módulo não encontrado.");

        var moduloAtualizado = new EntidadeModuloCurso(
            moduloExistente.CursoId,
            dto.Titulo,
            dto.Descricao,
            dto.DuracaoMinutos,
            dto.Ordem)
        {
            Ativo = moduloExistente.Ativo
        };

        Result resultadoValidacao = ValidarEntidade(moduloAtualizado);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        if (moduloExistente.Ativo)
        {
            Result resultadoOrdem = ValidarOrdemUnica(
                moduloAtualizado,
                moduloExistente.Id);

            if (resultadoOrdem.IsFailed)
                return resultadoOrdem;

            Result resultadoDuracao = ValidarDuracaoDoCurso(
                curso,
                moduloAtualizado,
                moduloExistente.Id);

            if (resultadoDuracao.IsFailed)
                return resultadoDuracao;
        }

        return repositorioModuloCurso.Editar(dto.Id, moduloAtualizado)
            ? Result.Ok()
            : Falha(string.Empty, "Não foi possível editar o módulo.");
    }

    public Result Desativar(Guid moduloCursoId)
    {
        EntidadeModuloCurso? modulo = repositorioModuloCurso.SelecionarPorId(
            moduloCursoId);

        if (modulo == null)
            return Falha(nameof(moduloCursoId), "Módulo não encontrado.");

        if (!modulo.Ativo)
            return Result.Ok();

        EntidadeModuloCurso moduloDesativado = CopiarModulo(modulo);
        moduloDesativado.Desativar();

        return repositorioModuloCurso.Editar(
            moduloCursoId,
            moduloDesativado)
            ? Result.Ok()
            : Falha(string.Empty, "Não foi possível desativar o módulo.");
    }

    public Result Reativar(Guid moduloCursoId)
    {
        EntidadeModuloCurso? modulo = repositorioModuloCurso.SelecionarPorId(
            moduloCursoId);

        if (modulo == null)
            return Falha(nameof(moduloCursoId), "Módulo não encontrado.");

        if (modulo.Ativo)
            return Result.Ok();

        Curso? curso = repositorioCurso.SelecionarPorId(modulo.CursoId);

        if (curso == null)
            return Falha(string.Empty, "Curso do módulo não encontrado.");

        EntidadeModuloCurso moduloReativado = CopiarModulo(modulo);
        moduloReativado.Reativar();

        Result resultadoOrdem = ValidarOrdemUnica(
            moduloReativado,
            modulo.Id);

        if (resultadoOrdem.IsFailed)
            return resultadoOrdem;

        Result resultadoDuracao = ValidarDuracaoDoCurso(
            curso,
            moduloReativado,
            modulo.Id);

        if (resultadoDuracao.IsFailed)
            return resultadoDuracao;

        return repositorioModuloCurso.Editar(
            moduloCursoId,
            moduloReativado)
            ? Result.Ok()
            : Falha(string.Empty, "Não foi possível reativar o módulo.");
    }

    public ModuloCursoDto? SelecionarPorId(Guid moduloCursoId)
    {
        EntidadeModuloCurso? modulo = repositorioModuloCurso.SelecionarPorId(
            moduloCursoId);

        return modulo == null ? null : MapearModulo(modulo);
    }

    public List<ModuloCursoDto> SelecionarPorCurso(
        Guid cursoId,
        bool incluirInativos = false)
    {
        return repositorioModuloCurso
            .Filtrar(modulo =>
                modulo.CursoId == cursoId &&
                (incluirInativos || modulo.Ativo))
            .OrderBy(modulo => modulo.Ordem)
            .Select(MapearModulo)
            .ToList();
    }

    private Result ValidarOrdemUnica(
        EntidadeModuloCurso modulo,
        Guid? moduloIgnoradoId = null)
    {
        bool ordemExistente = repositorioModuloCurso.Filtrar(registrado =>
            registrado.CursoId == modulo.CursoId &&
            registrado.Ativo &&
            registrado.Ordem == modulo.Ordem &&
            registrado.Id != moduloIgnoradoId).Any();

        return ordemExistente
            ? Falha(nameof(modulo.Ordem),
                "Já existe um módulo ativo com esta ordem no curso.")
            : Result.Ok();
    }

    private Result ValidarDuracaoDoCurso(
        Curso curso,
        EntidadeModuloCurso modulo,
        Guid? moduloIgnoradoId = null)
    {
        long duracaoRegistrada = repositorioModuloCurso
            .Filtrar(registrado =>
                registrado.CursoId == curso.Id &&
                registrado.Ativo &&
                registrado.Id != moduloIgnoradoId)
            .Sum(registrado => (long)registrado.DuracaoMinutos);

        long duracaoMaxima = curso.cargaHoraria * 60L;

        if (duracaoRegistrada + modulo.DuracaoMinutos > duracaoMaxima)
        {
            return Falha(
                nameof(modulo.DuracaoMinutos),
                "A duração total dos módulos não pode ultrapassar a carga horária do curso.");
        }

        return Result.Ok();
    }

    private static EntidadeModuloCurso CopiarModulo(
        EntidadeModuloCurso modulo)
    {
        return new EntidadeModuloCurso(
            modulo.CursoId,
            modulo.Titulo,
            modulo.Descricao,
            modulo.DuracaoMinutos,
            modulo.Ordem)
        {
            Ativo = modulo.Ativo
        };
    }

    private static ModuloCursoDto MapearModulo(
        EntidadeModuloCurso modulo)
    {
        return new ModuloCursoDto(
            modulo.Id,
            modulo.CursoId,
            modulo.Titulo,
            modulo.Descricao,
            modulo.DuracaoMinutos,
            modulo.Ordem,
            modulo.Ativo);
    }
}