using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloConteudoCurso;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;

[Authorize(Roles = "ADM")]
[Route("ModuloConteudoCurso/Apresentacao/[action]")]
public sealed class ConteudoCursoController(
    ServicoModuloCurso servicoModuloCurso,
    IMapper mapeador
) : Controller
{
    [HttpPost]
    public ActionResult Cadastrar(
        CadastrarModuloCursoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return RedirecionarParaEdicaoDoCurso(
                viewModel.CursoId,
                "Verifique os dados informados no módulo.");

        Result resultado = servicoModuloCurso.Cadastrar(
            mapeador.Map<CadastrarModuloCursoDto>(viewModel));

        if (resultado.IsFailed)
            return RedirecionarParaEdicaoDoCurso(
                viewModel.CursoId,
                resultado.Errors.First().Message);

        TempData["MensagemSucesso"] =
            "Módulo cadastrado com sucesso.";

        return RedirecionarParaEdicaoDoCurso(viewModel.CursoId);
    }

    [HttpPost]
    public ActionResult Editar(EditarModuloCursoViewModel viewModel)
    {
        ModuloCursoDto? modulo = servicoModuloCurso.SelecionarPorId(
            viewModel.Id);

        if (modulo == null)
            return NotFound();

        if (!ModelState.IsValid)
            return RedirecionarParaEdicaoDoCurso(
                modulo.CursoId,
                "Verifique os dados informados no módulo.");

        Result resultado = servicoModuloCurso.Editar(
            mapeador.Map<EditarModuloCursoDto>(viewModel));

        if (resultado.IsFailed)
            return RedirecionarParaEdicaoDoCurso(
                modulo.CursoId,
                resultado.Errors.First().Message);

        TempData["MensagemSucesso"] =
            "Módulo atualizado com sucesso.";

        return RedirecionarParaEdicaoDoCurso(modulo.CursoId);
    }

    [HttpPost]
    public ActionResult Desativar(Guid id)
    {
        return AlterarSituacaoDoModulo(id, reativar: false);
    }

    [HttpPost]
    public ActionResult Reativar(Guid id)
    {
        return AlterarSituacaoDoModulo(id, reativar: true);
    }

    private ActionResult AlterarSituacaoDoModulo(
        Guid moduloCursoId,
        bool reativar)
    {
        ModuloCursoDto? modulo = servicoModuloCurso.SelecionarPorId(
            moduloCursoId);

        if (modulo == null)
            return NotFound();

        Result resultado = reativar
            ? servicoModuloCurso.Reativar(moduloCursoId)
            : servicoModuloCurso.Desativar(moduloCursoId);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] =
                resultado.Errors.First().Message;
        }
        else
        {
            TempData["MensagemSucesso"] = reativar
                ? "Módulo reativado com sucesso."
                : "Módulo desativado com sucesso.";
        }

        return RedirecionarParaEdicaoDoCurso(modulo.CursoId);
    }

    private ActionResult RedirecionarParaEdicaoDoCurso(
        Guid cursoId,
        string? mensagemErro = null)
    {
        if (!string.IsNullOrWhiteSpace(mensagemErro))
            TempData["MensagemErro"] = mensagemErro;

        return RedirectToAction(
            "EditarCurso",
            "ADM",
            new { id = cursoId });
    }
}