using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;

public sealed class ServicoPresencaAluno : ServicoBase<PresencaAluno>
{
    private readonly IRepositorioAluno repositorioAluno;
    private readonly IRepositorioPresencaAluno repositorioPresencaAluno;
    private readonly IRepositorioMatricula repositorioMatricula;

    public ServicoPresencaAluno(
        IRepositorioAluno repositorioAluno,
        IRepositorioPresencaAluno repositorioPresencaAluno,
        IRepositorioMatricula repositorioMatricula)
    {
        this.repositorioAluno = repositorioAluno;
        this.repositorioPresencaAluno = repositorioPresencaAluno;
        this.repositorioMatricula = repositorioMatricula;
    }

    public Result Registrar(RegistrarPresencaAlunoDto dto)
    {
        Matricula? matricula = SelecionarMatriculaDoAluno(dto.MatriculaId);

        if (matricula == null)
            return Falha(nameof(dto.MatriculaId), "Matrícula de aluno não encontrada.");

        bool presencaJaRegistrada = repositorioPresencaAluno.Filtrar(presenca =>
            presenca.MatriculaId == dto.MatriculaId &&
            presenca.DataAula.Date == dto.DataAula.Date).Any();

        if (presencaJaRegistrada)
            return Falha(nameof(dto.DataAula), "A presença desta aula já foi registrada.");

        var presenca = new PresencaAluno(
            matricula.AlunoId,
            matricula.Id,
            dto.DataAula,
            dto.Presente
        );

        Result resultadoValidacao = ValidarPresenca(presenca, matricula);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioPresencaAluno.Cadastrar(presenca);

        return Result.Ok();
    }

    public Result Editar(EditarPresencaAlunoDto dto)
    {
        PresencaAluno? presencaExistente =
            repositorioPresencaAluno.SelecionarPorId(dto.Id);

        if (presencaExistente == null)
            return Falha(nameof(dto.Id), "Presença não encontrada.");

        Matricula? matricula = SelecionarMatriculaDoAluno(
            presencaExistente.MatriculaId);

        if (matricula == null)
            return Falha(nameof(dto.Id), "Matrícula de aluno não encontrada.");

        bool presencaJaRegistrada = repositorioPresencaAluno.Filtrar(presenca =>
            presenca.Id != dto.Id &&
            presenca.MatriculaId == matricula.Id &&
            presenca.DataAula.Date == dto.DataAula.Date).Any();

        if (presencaJaRegistrada)
            return Falha(nameof(dto.DataAula), "A presença desta aula já foi registrada.");

        var presencaAtualizada = new PresencaAluno(
            presencaExistente.AlunoId,
            presencaExistente.MatriculaId,
            dto.DataAula,
            dto.Presente
        );

        Result resultadoValidacao =
            ValidarPresenca(presencaAtualizada, matricula);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        return repositorioPresencaAluno.Editar(dto.Id, presencaAtualizada)
            ? Result.Ok()
            : Falha(string.Empty, "Não foi possível editar a presença.");
    }

    public Result Excluir(Guid presencaId)
    {
        return repositorioPresencaAluno.Excluir(presencaId)
            ? Result.Ok()
            : Falha(nameof(presencaId), "Presença não encontrada.");
    }

    public List<PresencaAlunoDto> SelecionarPorMatricula(Guid matriculaId)
    {
        return repositorioPresencaAluno.Filtrar(presenca =>
                presenca.MatriculaId == matriculaId)
            .OrderBy(presenca => presenca.DataAula)
            .Select(MapearPresenca)
            .ToList();
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

    private static Result ValidarPresenca(
        PresencaAluno presenca,
        Matricula matricula)
    {
        Result resultadoValidacao = ValidarEntidade(presenca);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        if (presenca.DataAula.Date < matricula.DataMatricula.Date)
            return Falha(nameof(presenca.DataAula), "A aula não pode ser anterior à matrícula.");

        return Result.Ok();
    }

    private static PresencaAlunoDto MapearPresenca(PresencaAluno presenca)
    {
        return new PresencaAlunoDto(
            presenca.Id,
            presenca.AlunoId,
            presenca.MatriculaId,
            presenca.DataAula,
            presenca.Presente
        );
    }
}