using System.Security.Claims;
using AutoMapper;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;

[Authorize]
[Route("ModuloConteudoCurso/Apresentacao/[action]")]
public sealed class ProgressoModuloController(
    ServicoProgressoModuloAluno servicoProgresso,
    ServicoMatricula servicoMatricula,
    IMapper mapeador
) : Controller
{
    [HttpGet]
    [Authorize(Roles = "Aluno")]
    public ActionResult ProgressoAluno(Guid matriculaId)
    {
        if (!TentarObterUsuarioId(out Guid alunoId))
            return Forbid();

        ResumoProgressoModuloAlunoDto? progresso = servicoProgresso
            .SelecionarProgressoDoAluno(matriculaId, alunoId);

        if (progresso == null)
            return NotFound();

        return View(
            mapeador.Map<ResumoProgressoModuloAlunoViewModel>(progresso));
    }

    [HttpPost]
    [Authorize(Roles = "Aluno")]
    public ActionResult AtualizarConclusao(
        AtualizarConclusaoModuloAlunoViewModel viewModel)
    {
        if (!TentarObterUsuarioId(out Guid alunoId))
            return Forbid();

        if (!ModelState.IsValid)
            return RedirectToAction(
                nameof(ProgressoAluno),
                new { matriculaId = viewModel.MatriculaId });

        Result resultado = servicoProgresso.AtualizarConclusaoDoAluno(
            mapeador.Map<AtualizarConclusaoModuloAlunoDto>(viewModel),
            alunoId);

        if (resultado.IsFailed)
        {
            TempData["MensagemProgressoErro"] =
                resultado.Errors.First().Message;
        }
        else
        {
            TempData["MensagemProgressoSucesso"] =
                "Progresso atualizado com sucesso.";
        }

        return RedirectToAction(
            nameof(ProgressoAluno),
            new { matriculaId = viewModel.MatriculaId });
    }

    [HttpGet]
    [Authorize(Roles = "ADM,Professor")]
    public ActionResult Consultar(Guid matriculaId, Guid turmaId)
    {
        if (User.IsInRole("Professor") &&
            !ProfessorPodeConsultar(turmaId, matriculaId))
        {
            return Forbid();
        }

        ResumoProgressoModuloAlunoDto? progresso = servicoProgresso
            .SelecionarProgressoDaMatricula(matriculaId);

        if (progresso == null)
            return NotFound();

        return View(
            mapeador.Map<ResumoProgressoModuloAlunoViewModel>(progresso));
    }

    private bool ProfessorPodeConsultar(
        Guid turmaId,
        Guid matriculaId)
    {
        if (!TentarObterUsuarioId(out Guid professorId))
            return false;

        TurmaDetalheDto? turma = servicoMatricula
            .ObterDetalhesTurmaDoProfessor(turmaId, professorId);

        return turma?.Matriculas.Any(matricula =>
            matricula.MatriculaId == matriculaId) == true;
    }

    private bool TentarObterUsuarioId(out Guid usuarioId)
    {
        string? identificador = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        return Guid.TryParse(identificador, out usuarioId);
    }
}