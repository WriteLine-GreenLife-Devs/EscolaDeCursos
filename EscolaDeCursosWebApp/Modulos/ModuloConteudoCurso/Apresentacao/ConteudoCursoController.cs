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
    [HttpGet]
    public ActionResult Listar(Guid cursoId)
    {
        var viewModel = new ListaModulosCursoViewModel
        {
            CursoId = cursoId,
            Modulos = mapeador.Map<List<ModuloCursoViewModel>>(
                servicoModuloCurso.SelecionarPorCurso(
                    cursoId,
                    incluirInativos: true))
        };

        return View(viewModel);
    }

    [HttpGet]
    public ActionResult Cadastrar(Guid cursoId)
    {
        return View(new CadastrarModuloCursoViewModel
        {
            CursoId = cursoId
        });
    }

    [HttpPost]
    public ActionResult Cadastrar(
        CadastrarModuloCursoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Result resultado = servicoModuloCurso.Cadastrar(
            mapeador.Map<CadastrarModuloCursoDto>(viewModel));

        if (resultado.IsFailed)
        {
            AdicionarErros(resultado);
            return View(viewModel);
        }

        TempData["MensagemModuloSucesso"] =
            "Módulo cadastrado com sucesso.";

        return RedirectToAction(
            nameof(Listar),
            new { cursoId = viewModel.CursoId });
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        ModuloCursoDto? modulo = servicoModuloCurso.SelecionarPorId(id);

        if (modulo == null)
            return NotFound();

        return View(
            mapeador.Map<EditarModuloCursoViewModel>(modulo));
    }

    [HttpPost]
    public ActionResult Editar(EditarModuloCursoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Result resultado = servicoModuloCurso.Editar(
            mapeador.Map<EditarModuloCursoDto>(viewModel));

        if (resultado.IsFailed)
        {
            AdicionarErros(resultado);
            return View(viewModel);
        }

        TempData["MensagemModuloSucesso"] =
            "Módulo atualizado com sucesso.";

        return RedirectToAction(
            nameof(Listar),
            new { cursoId = viewModel.CursoId });
    }

    [HttpPost]
    public ActionResult Desativar(Guid id)
    {
        return AlterarSituacao(id, reativar: false);
    }

    [HttpPost]
    public ActionResult Reativar(Guid id)
    {
        return AlterarSituacao(id, reativar: true);
    }

    private ActionResult AlterarSituacao(
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
            TempData["MensagemModuloErro"] =
                resultado.Errors.First().Message;
        }
        else
        {
            TempData["MensagemModuloSucesso"] = reativar
                ? "Módulo reativado com sucesso."
                : "Módulo desativado com sucesso.";
        }

        return RedirectToAction(
            nameof(Listar),
            new { cursoId = modulo.CursoId });
    }

    private void AdicionarErros(Result resultado)
    {
        foreach (IError erro in resultado.Errors)
        {
            string campo = erro.Metadata.TryGetValue(
                "Campo",
                out object? valorCampo)
                ? valorCampo?.ToString() ?? string.Empty
                : string.Empty;

            ModelState.AddModelError(campo, erro.Message);
        }
    }
}