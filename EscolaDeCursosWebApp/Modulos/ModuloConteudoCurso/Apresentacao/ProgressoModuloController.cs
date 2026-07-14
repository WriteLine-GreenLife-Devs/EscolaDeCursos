using System.Security.Claims;
using AutoMapper;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;

[Authorize]
[Route("ModuloConteudoCurso/Apresentacao/[action]")]
public sealed class ProgressoModuloController(
    ServicoProgressoModuloAluno servicoProgresso,
    IMapper mapeador
) : Controller
{
    [HttpPost]
    [Authorize(Roles = "Aluno")]
    public ActionResult AtualizarConclusao(
        AtualizarConclusaoModuloAlunoViewModel viewModel)
    {
        if (!TentarObterAlunoId(out Guid alunoId))
            return Forbid();

        if (!ModelState.IsValid)
        {
            TempData["MensagemProgressoErro"] =
                "Não foi possível atualizar o progresso.";

            return RedirectToAction(
                "DetalhesMatricula",
                "Aluno",
                new { id = viewModel.MatriculaId });
        }

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
            "DetalhesMatricula",
            "Aluno",
            new { id = viewModel.MatriculaId });
    }

    private bool TentarObterAlunoId(out Guid alunoId)
    {
        string? identificador = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        return Guid.TryParse(identificador, out alunoId);
    }
}