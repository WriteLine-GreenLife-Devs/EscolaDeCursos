using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;

public sealed class ServicoPresencaAluno : ServicoBase<PresencaAluno>
{
    private readonly IRepositorioAluno repositorioAluno;
    private readonly IRepositorioPresencaAluno repositorioPresencaAluno;
    private readonly IRepositorioMatricula repositorioMatricula;
    private readonly IRepositorioTurma repositorioTurma;

    public ServicoPresencaAluno(
        IRepositorioAluno repositorioAluno,
        IRepositorioPresencaAluno repositorioPresencaAluno,
        IRepositorioMatricula repositorioMatricula,
        IRepositorioTurma repositorioTurma)
    {
        this.repositorioAluno = repositorioAluno;
        this.repositorioPresencaAluno = repositorioPresencaAluno;
        this.repositorioMatricula = repositorioMatricula;
        this.repositorioTurma = repositorioTurma;
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

    public Result SalvarChamadaDoProfessor(
        SalvarChamadaAlunoDto dto,
        Guid professorId)
    {
        Turma? turma = repositorioTurma.SelecionarPorId(dto.TurmaId);

        if (turma == null || turma.instrutorId != professorId)
            return Falha(nameof(dto.TurmaId), "A turma não pertence a este professor.");

        return SalvarChamada(dto, turma);
    }

    public Result SalvarChamadaDoAdministrador(SalvarChamadaAlunoDto dto)
    {
        Turma? turma = repositorioTurma.SelecionarPorId(dto.TurmaId);

        if (turma == null)
            return Falha(nameof(dto.TurmaId), "Turma não encontrada.");

        return SalvarChamada(dto, turma);
    }

    private Result SalvarChamada(
        SalvarChamadaAlunoDto dto,
        Turma turma)
    {

        DateTime dataAula = dto.DataAula.Date;

        if (dataAula < turma.dataInicio.Date || dataAula > turma.dataFim.Date)
            return Falha(nameof(dto.DataAula), "A data da aula deve estar dentro do período da turma.");

        if (dataAula > DateTime.Today)
            return Falha(nameof(dto.DataAula), "A presença não pode ser registrada em uma data futura.");

        List<Matricula> matriculasDaTurma = repositorioMatricula
            .Filtrar(matricula => matricula.TurmaId == dto.TurmaId);

        if (matriculasDaTurma.Count == 0)
            return Falha(nameof(dto.TurmaId), "A turma não possui alunos matriculados.");

        if (dto.Alunos
            .GroupBy(aluno => aluno.MatriculaId)
            .Any(grupo => grupo.Count() > 1))
        {
            return Falha(nameof(dto.Alunos), "A chamada possui matrículas duplicadas.");
        }

        HashSet<Guid> idsMatriculasDaTurma = matriculasDaTurma
            .Select(matricula => matricula.Id)
            .ToHashSet();

        if (dto.Alunos.Any(aluno =>
            !idsMatriculasDaTurma.Contains(aluno.MatriculaId)))
        {
            return Falha(nameof(dto.Alunos), "A chamada contém uma matrícula que não pertence à turma.");
        }

        List<Matricula> matriculasElegiveis = matriculasDaTurma
            .Where(matricula => matricula.DataMatricula.Date <= dataAula)
            .ToList();

        if (matriculasElegiveis.Count == 0)
            return Falha(nameof(dto.DataAula), "Não havia alunos matriculados na data da aula.");

        Dictionary<Guid, PresencaChamadaAlunoDto> alunosInformados = dto.Alunos
            .Where(aluno => matriculasElegiveis.Any(matricula =>
                matricula.Id == aluno.MatriculaId))
            .ToDictionary(aluno => aluno.MatriculaId);

        if (alunosInformados.Count != matriculasElegiveis.Count)
            return Falha(nameof(dto.Alunos), "Informe a presença de todos os alunos matriculados na data da aula.");

        List<PresencaAluno> presencasExistentes = repositorioPresencaAluno
            .Filtrar(presenca =>
                presenca.DataAula.Date == dataAula &&
                idsMatriculasDaTurma.Contains(presenca.MatriculaId));

        List<PresencaAluno> presencasNovas = [];

        foreach (Matricula matricula in matriculasElegiveis)
        {
            PresencaChamadaAlunoDto alunoInformado =
                alunosInformados[matricula.Id];

            var presencaAtualizada = new PresencaAluno(
                matricula.AlunoId,
                matricula.Id,
                dataAula,
                alunoInformado.Presente);

            Result resultadoValidacao = ValidarPresenca(
                presencaAtualizada,
                matricula);

            if (resultadoValidacao.IsFailed)
                return resultadoValidacao;

            PresencaAluno? presencaExistente = presencasExistentes
                .SingleOrDefault(presenca =>
                    presenca.MatriculaId == matricula.Id);

            if (presencaExistente == null)
                presencasNovas.Add(presencaAtualizada);
            else
                presencaExistente.Atualizar(presencaAtualizada);
        }

        repositorioPresencaAluno.SalvarAlteracoes(presencasNovas);

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