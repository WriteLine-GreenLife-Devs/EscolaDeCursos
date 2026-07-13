using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

[Authorize(Roles = "ADM")]
[Route("ModuloADM/Apresentacao/[action]")]
public class ADMController(ServicoUsuario servicoUsuario, IRepositorioUsuario repositorioUsuario) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public ActionResult ListarAlunos()
    {
        var usuarios = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Aluno)
            .Select(u => new UsuarioADMViewModel
            {
                Id = u.Id,
                Nome = u.nome,
                Email = u.email,
                Telefone = u.telefone
            })
            .ToList();

        return View("~/Modulos/ModuloADM/Apresentacao/Views/AlunoADM/Listar.cshtml", usuarios);
    }

    [HttpGet]
    public ActionResult CadastrarAluno()
    {
        return View("~/Modulos/ModuloADM/Apresentacao/Views/AlunoADM/Cadastrar.cshtml", new CadastrarUsuarioADMViewModel());
    }

    [HttpPost]
    public ActionResult CadastrarAluno(CadastrarUsuarioADMViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/AlunoADM/Cadastrar.cshtml", cadastrarVm);

        var dto = new CadastrarUsuarioDto(
            cadastrarVm.Nome,
            cadastrarVm.Email,
            cadastrarVm.Senha,
            cadastrarVm.Telefone,
            TipoUsuario.Aluno
        );

        var resultado = servicoUsuario.CadastrarUsuario(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível cadastrar o aluno. Verifique os dados e tente novamente.");
            return View("~/Modulos/ModuloADM/Apresentacao/Views/AlunoADM/Cadastrar.cshtml", cadastrarVm);
        }

        return RedirectToAction("ListarAlunos");
    }

    [HttpGet]
    public ActionResult EditarAluno(Guid id)
    {
        var usuario = repositorioUsuario.SelecionarPorId(id);

        if (usuario == null || usuario.tipoUsuario != TipoUsuario.Aluno)
            return NotFound();

        var editarVm = new EditarUsuarioADMViewModel
        {
            Id = usuario.Id,
            Nome = usuario.nome,
            Email = usuario.email,
            Telefone = usuario.telefone,
            Senha = string.Empty,
            ConfirmarSenha = string.Empty
        };

        return View("~/Modulos/ModuloADM/Apresentacao/Views/AlunoADM/Editar.cshtml", editarVm);
    }

    [HttpPost]
    public ActionResult EditarAluno(Guid id, EditarUsuarioADMViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/AlunoADM/Editar.cshtml", editarVm);

        var usuario = repositorioUsuario.SelecionarPorId(id);

        if (usuario == null || usuario.tipoUsuario != TipoUsuario.Aluno)
            return NotFound();

        bool emailDuplicado = repositorioUsuario.SelecionarTodos()
            .Any(u => u.Id != id && string.Equals(u.email, editarVm.Email, StringComparison.OrdinalIgnoreCase));
        bool telefoneDuplicado = repositorioUsuario.SelecionarTodos()
            .Any(u => u.Id != id && u.telefone == editarVm.Telefone);

        if (emailDuplicado)
            ModelState.AddModelError(nameof(editarVm.Email), "Já existe um usuário com esse email.");

        if (telefoneDuplicado)
            ModelState.AddModelError(nameof(editarVm.Telefone), "Já existe um usuário com esse telefone.");

        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/AlunoADM/Editar.cshtml", editarVm);

        string senhaAtualizada = string.IsNullOrWhiteSpace(editarVm.Senha)
            ? usuario.senha
            : editarVm.Senha;

        var entidadeAtualizada = new Usuario(
            editarVm.Nome,
            editarVm.Email,
            senhaAtualizada,
            editarVm.Telefone,
            TipoUsuario.Aluno
        );

        if (!repositorioUsuario.Editar(id, entidadeAtualizada))
        {
            TempData["MensagemErro"] = "Falha ao atualizar o aluno.";
            return RedirectToAction("ListarAlunos");
        }

        return RedirectToAction("ListarAlunos");
    }

    [HttpGet]
    public ActionResult ExcluirAluno(Guid id)
    {
        var usuario = repositorioUsuario.SelecionarPorId(id);

        if (usuario == null || usuario.tipoUsuario != TipoUsuario.Aluno)
            return NotFound();

        var excluirVm = new ExcluirUsuarioADMViewModel
        {
            Id = usuario.Id,
            Nome = usuario.nome,
            Email = usuario.email,
            Telefone = usuario.telefone
        };

        return View("~/Modulos/ModuloADM/Apresentacao/Views/AlunoADM/Excluir.cshtml", excluirVm);
    }

    [HttpPost]
    public ActionResult ExcluirAluno(Guid id, bool confirmado = true)
    {
        if (!confirmado)
            return RedirectToAction("ListarAlunos");

        if (!repositorioUsuario.Excluir(id))
            TempData["MensagemErro"] = "Falha ao excluir o aluno.";

        return RedirectToAction("ListarAlunos");
    }

    [HttpGet]
    public ActionResult ListarProfessores()
    {
        var usuarios = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Professor)
            .Select(u => new UsuarioADMViewModel
            {
                Id = u.Id,
                Nome = u.nome,
                Email = u.email,
                Telefone = u.telefone
            })
            .ToList();

        return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Listar.cshtml", usuarios);
    }

    [HttpGet]
    public ActionResult CadastrarProfessor()
    {
        return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Cadastrar.cshtml", new CadastrarUsuarioADMViewModel());
    }

    [HttpPost]
    public ActionResult CadastrarProfessor(CadastrarUsuarioADMViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Cadastrar.cshtml", cadastrarVm);

        var dto = new CadastrarUsuarioDto(
            cadastrarVm.Nome,
            cadastrarVm.Email,
            cadastrarVm.Senha,
            cadastrarVm.Telefone,
            TipoUsuario.Professor
        );

        var resultado = servicoUsuario.CadastrarUsuario(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível cadastrar o professor. Verifique os dados e tente novamente.");
            return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Cadastrar.cshtml", cadastrarVm);
        }

        return RedirectToAction("ListarProfessores");
    }

    [HttpGet]
    public ActionResult EditarProfessor(Guid id)
    {
        var usuario = repositorioUsuario.SelecionarPorId(id);

        if (usuario == null || usuario.tipoUsuario != TipoUsuario.Professor)
            return NotFound();

        var editarVm = new EditarUsuarioADMViewModel
        {
            Id = usuario.Id,
            Nome = usuario.nome,
            Email = usuario.email,
            Telefone = usuario.telefone,
            Senha = string.Empty,
            ConfirmarSenha = string.Empty
        };

        return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Editar.cshtml", editarVm);
    }

    [HttpPost]
    public ActionResult EditarProfessor(Guid id, EditarUsuarioADMViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Editar.cshtml", editarVm);

        var usuario = repositorioUsuario.SelecionarPorId(id);

        if (usuario == null || usuario.tipoUsuario != TipoUsuario.Professor)
            return NotFound();

        bool emailDuplicado = repositorioUsuario.SelecionarTodos()
            .Any(u => u.Id != id && string.Equals(u.email, editarVm.Email, StringComparison.OrdinalIgnoreCase));
        bool telefoneDuplicado = repositorioUsuario.SelecionarTodos()
            .Any(u => u.Id != id && u.telefone == editarVm.Telefone);

        if (emailDuplicado)
            ModelState.AddModelError(nameof(editarVm.Email), "Já existe um usuário com esse email.");

        if (telefoneDuplicado)
            ModelState.AddModelError(nameof(editarVm.Telefone), "Já existe um usuário com esse telefone.");

        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Editar.cshtml", editarVm);

        string senhaAtualizada = string.IsNullOrWhiteSpace(editarVm.Senha)
            ? usuario.senha
            : editarVm.Senha;

        var entidadeAtualizada = new Usuario(
            editarVm.Nome,
            editarVm.Email,
            senhaAtualizada,
            editarVm.Telefone,
            TipoUsuario.Professor
        );

        if (!repositorioUsuario.Editar(id, entidadeAtualizada))
        {
            TempData["MensagemErro"] = "Falha ao atualizar o professor.";
            return RedirectToAction("ListarProfessores");
        }

        return RedirectToAction("ListarProfessores");
    }

    [HttpGet]
    public ActionResult ExcluirProfessor(Guid id)
    {
        var usuario = repositorioUsuario.SelecionarPorId(id);

        if (usuario == null || usuario.tipoUsuario != TipoUsuario.Professor)
            return NotFound();

        var excluirVm = new ExcluirUsuarioADMViewModel
        {
            Id = usuario.Id,
            Nome = usuario.nome,
            Email = usuario.email,
            Telefone = usuario.telefone
        };

        return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Excluir.cshtml", excluirVm);
    }

    [HttpPost]
    public ActionResult ExcluirProfessor(Guid id, bool confirmado = true)
    {
        if (!confirmado)
            return RedirectToAction("ListarProfessores");

        if (!repositorioUsuario.Excluir(id))
            TempData["MensagemErro"] = "Falha ao excluir o professor.";

        return RedirectToAction("ListarProfessores");
    }
}
