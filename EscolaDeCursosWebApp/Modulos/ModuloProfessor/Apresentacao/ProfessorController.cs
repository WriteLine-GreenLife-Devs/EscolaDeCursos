using System.Security.Claims;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Apresentacao;

[Authorize(Roles = "Professor")]
[Route("ModuloProfessor/Apresentacao/[action]")]
public sealed class ProfessorController(
    ServicoTurma servicoTurma,
    ServicoMatricula servicoMatricula
) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        if (!TentarObterProfessorId(out Guid professorId))
            return Forbid();

        List<TurmaProfessorViewModel> turmas = servicoTurma
            .SelecionarPorProfessor(professorId)
            .Select(turma => new TurmaProfessorViewModel(
                turma.Id,
                turma.Nome,
                turma.CursoNome,
                turma.DataInicio,
                turma.DataFim,
                turma.HorarioTurno,
                turma.Status,
                turma.TotalAlunos))
            .ToList();

        var viewModel = new PainelProfessorViewModel
        {
            NomeProfessor = User.Identity?.Name ?? "Professor",
            Turmas = turmas
        };

        return View(viewModel);
    }

    [HttpGet]
    public ActionResult DetalhesTurma(Guid id)
    {
        if (!TentarObterProfessorId(out Guid professorId))
            return Forbid();

        TurmaDetalheDto? turma = servicoMatricula
            .ObterDetalhesTurmaDoProfessor(id, professorId);

        if (turma == null)
            return NotFound();

        var viewModel = new DetalhesTurmaProfessorViewModel
        {
            TurmaId = turma.TurmaId,
            TurmaNome = turma.TurmaNome,
            VagasMaximas = turma.VagasMaximas,
            Alunos = turma.Matriculas
                .Select(matricula => new AlunoTurmaProfessorViewModel(
                    matricula.MatriculaId,
                    matricula.AlunoNome,
                    matricula.DataMatricula,
                    matricula.Situacao))
                .OrderBy(aluno => aluno.Nome)
                .ToList()
        };

        return View(viewModel);
    }

    private bool TentarObterProfessorId(out Guid professorId)
    {
        string? identificador = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(identificador, out professorId);
    }
}
