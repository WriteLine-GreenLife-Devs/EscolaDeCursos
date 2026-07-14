using System.Security.Claims;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloProfessor.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Apresentacao;

[Authorize(Roles = "Professor")]
[Route("ModuloProfessor/Apresentacao/[action]")]
public sealed class ProfessorController(
    ServicoTurma servicoTurma,
    ServicoMatricula servicoMatricula,
    ServicoProfessor servicoProfessor
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

        DetalhesProfessorDto? perfilDto =
            servicoProfessor.SelecionarDadosParaPerfil(professorId);

        DetalhesProfessorViewModel? perfil = perfilDto == null
            ? null
            : new DetalhesProfessorViewModel(
                perfilDto.Id,
                perfilDto.Nome,
                perfilDto.Email,
                perfilDto.Telefone,
                perfilDto.Bio,
                perfilDto.Especialidades,
                perfilDto.DataContratacao);

        var viewModel = new PainelProfessorViewModel
        {
            NomeProfessor = User.Identity?.Name ?? "Professor",
            Perfil = perfil,
            PerfilCadastrado = servicoProfessor.SelecionarPorId(professorId) != null,
            Turmas = turmas
        };

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult SalvarPerfil(EditarProfessorPerfilViewModel viewModel)
    {
        if (!TentarObterProfessorId(out Guid professorId))
            return Forbid();

        if (!ModelState.IsValid)
        {
            TempData["MensagemPerfilErro"] = "Verifique os dados informados.";
            return RedirectToAction(nameof(Index), new { abrirPerfil = true });
        }

        Result resultado;

        if (servicoProfessor.SelecionarPorId(professorId) == null)
        {
            resultado = servicoProfessor.Cadastrar(
                new CadastrarProfessorPerfilDto(
                    professorId,
                    viewModel.Bio,
                    viewModel.Especialidades,
                    viewModel.DataContratacao));
        }
        else
        {
            resultado = servicoProfessor.Editar(
                new EditarProfessorPerfilDto(
                    professorId,
                    viewModel.Bio,
                    viewModel.Especialidades,
                    viewModel.DataContratacao));
        }

        if (resultado.IsFailed)
        {
            TempData["MensagemPerfilErro"] = resultado.Errors.First().Message;
            return RedirectToAction(nameof(Index), new { abrirPerfil = true });
        }

        TempData["MensagemPerfilSucesso"] = "Perfil atualizado com sucesso.";
        return RedirectToAction(nameof(Index), new { abrirPerfil = true });
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