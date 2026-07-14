using AutoMapper;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Aplicacao;
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
            return RedirecionarParaListaDeCursos(
                "Verifique os dados informados no módulo.");

        Result resultado = servicoModuloCurso.Cadastrar(
            mapeador.Map<CadastrarModuloCursoDto>(viewModel));

        if (resultado.IsFailed)
            return RedirecionarParaListaDeCursos(
                resultado.Errors.First().Message);

        TempData["MensagemSucesso"] =
            "Módulo cadastrado com sucesso.";

        return RedirecionarParaListaDeCursos();
    }

    [HttpPost]
    public ActionResult Editar(EditarModuloCursoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return RedirecionarParaListaDeCursos(
                "Verifique os dados informados no módulo.");

        Result resultado = servicoModuloCurso.Editar(
            mapeador.Map<EditarModuloCursoDto>(viewModel));

        if (resultado.IsFailed)
            return RedirecionarParaListaDeCursos(
                resultado.Errors.First().Message);

        TempData["MensagemSucesso"] =
            "Módulo atualizado com sucesso.";

        return RedirecionarParaListaDeCursos();
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

        return RedirecionarParaListaDeCursos();
    }

    private ActionResult RedirecionarParaListaDeCursos(
        string? mensagemErro = null)
    {
        if (!string.IsNullOrWhiteSpace(mensagemErro))
            TempData["MensagemErro"] = mensagemErro;

        return RedirectToAction("ListarCursos", "ADM");
    }
}