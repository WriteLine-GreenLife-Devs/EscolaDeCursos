using AutoMapper;
using EscolaDeCursosWebApp.Compartilhado.Apresentacao.Extensions;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EscolaDeCursosWebApp.Modulos.ModuloUsuario.Apresentacao;

[Route("ModuloUsuario/Apresentacao/[action]")]
public class UsuarioController(
    ServicoUsuario servicoUsuario,
    ServicoAluno servicoAluno,
    IMapper mapeador
) : Controller
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

        CadastrarAlunoDto dto = mapeador.Map<CadastrarAlunoDto>(cadastrarVm);

        Result resultado = servicoAluno.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(cadastrarVm);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public ActionResult Login(string Email, string Senha)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
        {
            TempData["LoginError"] = "Email e senha são obrigatórios.";
            return RedirectToAction("Index", "Home");
        }

        var usuario = servicoUsuario.AutenticarUsuario(Email, Senha);
        if (usuario == null)
        {
            // credenciais inválidas
            TempData["LoginError"] = "Credenciais inválidas.";
            return RedirectToAction("Index", "Home");
        }
        // criar claims e assinar o cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.nome ?? string.Empty),
            new Claim(ClaimTypes.Email, usuario.email ?? string.Empty),
            new Claim(ClaimTypes.Role, usuario.tipoUsuario.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).GetAwaiter().GetResult();

        return usuario.tipoUsuario switch
        {
            TipoUsuario.ADM =>
                Redirect("/ModuloADM/Apresentacao/Index"),

            TipoUsuario.Professor =>
                Redirect("/ModuloProfessor/Apresentacao/Index"),

            TipoUsuario.Aluno =>
                Redirect("/ModuloAluno/Apresentacao/Index"),

            _ =>
                RedirectToAction("Index", "Home")
        };
    }

    [HttpPost]
    public ActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();
        return RedirectToAction("Index", "Home");
    }
}