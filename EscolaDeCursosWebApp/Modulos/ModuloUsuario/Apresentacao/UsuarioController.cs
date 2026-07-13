using AutoMapper;
using EscolaDeCursosWebApp.Compartilhado.Apresentacao.Extensions;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Modulos.ModuloUsuario.Apresentacao;

[Route("ModuloUsuario/Apresentacao/[action]")]
public class UsuarioController(ServicoUsuario servicoUsuario, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult CadastrarAluno()
    {
        CadastrarUsuarioViewModel cadastrarVm = new CadastrarUsuarioViewModel
        {
            Nome = string.Empty,
            Email = string.Empty,
            Senha = string.Empty,
            ConfirmarSenha = string.Empty,
            Telefone = string.Empty,
            TipoUsuario = TipoUsuario.Aluno
        };

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult CadastrarAluno(CadastrarUsuarioViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        cadastrarVm.TipoUsuario = TipoUsuario.Aluno;
        CadastrarUsuarioDto dto = mapeador.Map<CadastrarUsuarioDto>(cadastrarVm);

        Result resultado = servicoUsuario.CadastrarUsuario(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(cadastrarVm);
        }

        return RedirectToAction("Index", "Home");
    }
}