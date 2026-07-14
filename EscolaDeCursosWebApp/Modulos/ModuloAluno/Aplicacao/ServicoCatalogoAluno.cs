using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;

public sealed class ServicoCatalogoAluno(
    IRepositorioAluno repositorioAluno,
    IRepositorioCategoria repositorioCategoria,
    IRepositorioCurso repositorioCurso,
    IRepositorioTurma repositorioTurma,
    IRepositorioMatricula repositorioMatricula,
    IRepositorioUsuario repositorioUsuario)
{
    public List<CursoCatalogoAlunoDto> SelecionarCursos(Guid alunoId)
    {
        if (repositorioAluno.SelecionarPorId(alunoId) == null)
            return [];

        List<Matricula> matriculasAluno = repositorioMatricula
            .Filtrar(matricula => matricula.AlunoId == alunoId);

        return repositorioCurso
            .Filtrar(curso => curso.status == StatusCurso.Ativo)
            .OrderBy(curso => curso.nome)
            .Select(curso => MapearCurso(curso, matriculasAluno))
            .ToList();
    }

    private CursoCatalogoAlunoDto MapearCurso(
        Curso curso,
        List<Matricula> matriculasAluno)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(
            curso.categoriaId);

        List<TurmaCatalogoAlunoDto> turmas = repositorioTurma
            .Filtrar(turma => turma.cursoId == curso.Id)
            .OrderBy(turma => turma.dataInicio)
            .Select(turma => MapearTurma(turma, matriculasAluno))
            .ToList();

        return new CursoCatalogoAlunoDto(
            curso.Id,
            curso.nome,
            curso.descricao,
            curso.cargaHoraria,
            curso.nivelDificuldade,
            curso.valor,
            categoria?.nome ?? "Categoria não informada",
            turmas);
    }

    private TurmaCatalogoAlunoDto MapearTurma(
        Turma turma,
        List<Matricula> matriculasAluno)
    {
        int vagasOcupadas = repositorioMatricula
            .Filtrar(matricula =>
                matricula.TurmaId == turma.Id &&
                matricula.Situacao == SituacaoMatricula.Cursando)
            .Count;

        int vagasDisponiveis = Math.Max(
            turma.vagasMaximas - vagasOcupadas,
            0);

        bool alunoMatriculado = matriculasAluno.Any(matricula =>
            matricula.TurmaId == turma.Id);

        bool inscricaoDisponivel =
            turma.status == StatusTurma.InscricoesAbertas &&
            turma.dataFim.Date >= DateTime.Today &&
            vagasDisponiveis > 0 &&
            !alunoMatriculado;

        Usuario? professor = repositorioUsuario.SelecionarPorId(
            turma.instrutorId);

        return new TurmaCatalogoAlunoDto(
            turma.Id,
            turma.nome,
            turma.dataInicio,
            turma.dataFim,
            turma.HorarioTurno,
            turma.status,
            professor?.nome ?? "Professor não informado",
            vagasDisponiveis,
            alunoMatriculado,
            inscricaoDisponivel,
            ObterMotivoIndisponibilidade(
                turma,
                vagasDisponiveis,
                alunoMatriculado));
    }

    private static string ObterMotivoIndisponibilidade(
        Turma turma,
        int vagasDisponiveis,
        bool alunoMatriculado)
    {
        if (alunoMatriculado)
            return string.Empty;

        if (turma.status != StatusTurma.InscricoesAbertas ||
            turma.dataFim.Date < DateTime.Today)
        {
            return "Inscrições indisponíveis para esta turma.";
        }

        if (vagasDisponiveis == 0)
            return "Turma sem vagas disponíveis.";

        return string.Empty;
    }
}
