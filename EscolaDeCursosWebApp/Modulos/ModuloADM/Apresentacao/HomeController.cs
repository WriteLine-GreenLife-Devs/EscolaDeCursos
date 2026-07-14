using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloProfessor.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

[Authorize(Roles = "ADM")]
[Route("ModuloADM/Apresentacao/[action]")]
public class ADMController(
    ServicoUsuario servicoUsuario,
    ServicoProfessor servicoProfessor,
    ServicoAluno servicoAluno,
    IRepositorioUsuario repositorioUsuario,
    ServicoCategoria servicoCategoria,
    IRepositorioCategoria repositorioCategoria,
    ServicoCurso servicoCurso,
    IRepositorioCurso repositorioCurso,
    ServicoTurma servicoTurma,
    IRepositorioTurma repositorioTurma,
    ServicoMatricula servicoMatricula
) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public ActionResult ListarCategorias()
    {
        var categorias = repositorioCategoria.SelecionarTodos()
            .OrderBy(c => c.nome)
            .ToList();

        return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/Listar.cshtml", categorias);
    }

    [HttpGet]
    public ActionResult CadastrarCategoria()
    {
        return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/Cadastrar.cshtml", new CadastrarCategoriaDto(string.Empty, string.Empty, StatusCategoria.Ativo));
    }

    [HttpPost]
    public ActionResult CadastrarCategoria(CadastrarCategoriaDto cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/Cadastrar.cshtml", cadastrarVm);

        var resultado = servicoCategoria.CadastrarCategoria(cadastrarVm);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors.First().Message);
            return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/Cadastrar.cshtml", cadastrarVm);
        }

        return RedirectToAction("ListarCategorias");
    }

    [HttpGet]
    public ActionResult EditarCategoria(Guid id)
    {
        var categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return NotFound();

        var editarVm = new EditarCategoriaDto(
            categoria.Id,
            categoria.nome,
            categoria.descricao,
            categoria.status
        );

        return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/Editar.cshtml", editarVm);
    }

    [HttpPost]
    public ActionResult EditarCategoria(Guid id, EditarCategoriaDto editarVm)
    {
        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/Editar.cshtml", editarVm);

        var resultado = servicoCategoria.EditarCategoria(id, editarVm);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors.First().Message);
            return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/Editar.cshtml", editarVm);
        }

        return RedirectToAction("ListarCategorias");
    }

    [HttpGet]
    public ActionResult ExcluirCategoria(Guid id)
    {
        var categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return NotFound();

        var excluirVm = new ExcluirCategoriaDto(categoria.Id, categoria.nome, categoria.descricao, categoria.status);
        return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/Excluir.cshtml", excluirVm);
    }

    [HttpPost]
    public ActionResult ExcluirCategoria(Guid id, bool confirmado = true)
    {
        if (!confirmado)
            return RedirectToAction("ListarCategorias");

        if (!servicoCategoria.ExcluirCategoria(id))
            TempData["MensagemErro"] = "Falha ao excluir a categoria.";

        return RedirectToAction("ListarCategorias");
    }

    [HttpGet]
    public ActionResult ListarCursos()
    {
        var categorias = repositorioCategoria.SelecionarTodos()
            .ToDictionary(c => c.Id, c => c.nome);

        var cursos = repositorioCurso.SelecionarTodos()
            .OrderBy(c => c.nome)
            .Select(curso => new CursoADMViewModel
            {
                Id = curso.Id,
                Nome = curso.nome,
                Descricao = curso.descricao,
                CargaHoraria = curso.cargaHoraria,
                NivelDificuldade = curso.nivelDificuldade,
                Status = curso.status,
                Valor = curso.valor,
                CategoriaNome = categorias.TryGetValue(curso.categoriaId, out var nomeCategoria) ? nomeCategoria : "-"
            })
            .ToList();

        return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/Listar.cshtml", cursos);
    }

    [HttpGet]
    public ActionResult ListarCursoCategoria(Guid id)
    {
        var categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return NotFound();

        var cursos = repositorioCurso.SelecionarTodos()
            .Where(c => c.categoriaId == id)
            .OrderBy(c => c.nome)
            .Select(curso => new CursoADMViewModel
            {
                Id = curso.Id,
                Nome = curso.nome,
                Descricao = curso.descricao,
                CargaHoraria = curso.cargaHoraria,
                NivelDificuldade = curso.nivelDificuldade,
                Status = curso.status,
                Valor = curso.valor,
                CategoriaNome = categoria.nome
            })
            .ToList();

        ViewBag.CategoriaNome = categoria.nome;
        return View("~/Modulos/ModuloADM/Apresentacao/Views/CategoriaADM/ListarCursoCategoria.cshtml", cursos);
    }


    [HttpGet]
    public ActionResult CadastrarCurso()
    {
        ViewBag.Categorias = repositorioCategoria.SelecionarTodos().OrderBy(c => c.nome).ToList();
        return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/Cadastrar.cshtml", new CadastrarCursoDto());
    }

    [HttpPost]
    public ActionResult CadastrarCurso(CadastrarCursoDto cadastrarVm)
    {
        ViewBag.Categorias = repositorioCategoria.SelecionarTodos().OrderBy(c => c.nome).ToList();

        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/Cadastrar.cshtml", cadastrarVm);

        var resultado = servicoCurso.CadastrarCurso(cadastrarVm);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors.First().Message);
            return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/Cadastrar.cshtml", cadastrarVm);
        }

        return RedirectToAction("ListarCursos");
    }

    [HttpGet]
    public ActionResult EditarCurso(Guid id)
    {
        var curso = repositorioCurso.SelecionarPorId(id);

        if (curso == null)
            return NotFound();

        ViewBag.Categorias = repositorioCategoria.SelecionarTodos().OrderBy(c => c.nome).ToList();

        var editarVm = new EditarCursoDto(
            curso.Id,
            curso.nome,
            curso.descricao,
            curso.cargaHoraria,
            curso.nivelDificuldade,
            curso.status,
            curso.valor,
            curso.categoriaId
        );

        return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/Editar.cshtml", editarVm);
    }

    [HttpPost]
    public ActionResult EditarCurso(Guid id, EditarCursoDto editarVm)
    {
        ViewBag.Categorias = repositorioCategoria.SelecionarTodos().OrderBy(c => c.nome).ToList();

        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/Editar.cshtml", editarVm);

        var resultado = servicoCurso.EditarCurso(id, editarVm);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors.First().Message);
            return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/Editar.cshtml", editarVm);
        }

        return RedirectToAction("ListarCursos");
    }

    [HttpGet]
    public ActionResult ExcluirCurso(Guid id)
    {
        var curso = repositorioCurso.SelecionarPorId(id);

        if (curso == null)
            return NotFound();

        var categoria = repositorioCategoria.SelecionarPorId(curso.categoriaId);
        var excluirVm = new ExcluirCursoDto(
            curso.Id,
            curso.nome,
            curso.descricao,
            curso.cargaHoraria,
            curso.nivelDificuldade,
            curso.status,
            curso.valor,
            curso.categoriaId
        )
        {
            CategoriaNome = categoria?.nome ?? "-"
        };

        return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/Excluir.cshtml", excluirVm);
    }

    [HttpPost]
    public ActionResult ExcluirCurso(Guid id, bool confirmado = true)
    {
        if (!confirmado)
            return RedirectToAction("ListarCursos");

        if (!servicoCurso.ExcluirCurso(id))
            TempData["MensagemErro"] = "Falha ao excluir o curso.";

        return RedirectToAction("ListarCursos");
    }

    [HttpGet]
    public ActionResult ListarTurmaCurso(Guid id)
    {
        var curso = repositorioCurso.SelecionarPorId(id);

        if (curso == null)
            return NotFound();

        var professores = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Professor)
            .ToDictionary(u => u.Id, u => u.nome);

        var turmas = repositorioTurma.SelecionarTodos()
            .Where(t => t.cursoId == id)
            .OrderBy(t => t.nome)
            .Select(turma => new TurmaADMViewModel
            {
                Id = turma.Id,
                Nome = turma.nome,
                DataInicio = turma.dataInicio,
                DataFim = turma.dataFim,
                VagasMaximas = turma.vagasMaximas,
                HorarioTurno = turma.HorarioTurno,
                Status = turma.status,
                CursoNome = curso.nome,
                InstrutorNome = professores.TryGetValue(turma.instrutorId, out var nomeInstrutor) ? nomeInstrutor : "-"
            })
            .ToList();

        ViewBag.CursoNome = curso.nome;
        return View("~/Modulos/ModuloADM/Apresentacao/Views/CursoADM/ListarTurmaCurso.cshtml", turmas);
    }


    [HttpGet]
    public ActionResult ListarTurmas()
    {
        var cursos = repositorioCurso.SelecionarTodos().ToDictionary(c => c.Id, c => c.nome);
        var professores = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Professor)
            .ToDictionary(u => u.Id, u => u.nome);

        var turmas = repositorioTurma.SelecionarTodos()
            .OrderBy(t => t.nome)
            .Select(turma => new TurmaADMViewModel
            {
                Id = turma.Id,
                Nome = turma.nome,
                DataInicio = turma.dataInicio,
                DataFim = turma.dataFim,
                VagasMaximas = turma.vagasMaximas,
                HorarioTurno = turma.HorarioTurno,
                Status = turma.status,
                CursoNome = cursos.TryGetValue(turma.cursoId, out var nomeCurso) ? nomeCurso : "-",
                InstrutorNome = professores.TryGetValue(turma.instrutorId, out var nomeInstrutor) ? nomeInstrutor : "-"
            })
            .ToList();

        return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/Listar.cshtml", turmas);
    }

    [HttpGet]
    public ActionResult VerTurma(Guid id)
    {
        var detalhe = servicoMatricula.ObterDetalhesTurma(id);
        if (detalhe == null)
            return RedirectToAction("ListarTurmas");

        var alunos = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Aluno && u.ativo)
            .ToList();

        var alunosDisponiveis = alunos
            .Select(a => new { a.Id, a.nome })
            .ToList();

        var vm = new EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao.VerTurmaViewModel
        {
            TurmaId = detalhe.TurmaId,
            TurmaNome = detalhe.TurmaNome,
            InstrutorNome = detalhe.InstrutorNome,
            VagasMaximas = detalhe.VagasMaximas,
            Matriculas = detalhe.Matriculas.Select(m => new EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao.AlunoMatriculaViewModel
            {
                MatriculaId = m.MatriculaId,
                AlunoId = m.AlunoId,
                AlunoNome = m.AlunoNome,
                Situacao = m.Situacao,
                DataMatricula = m.DataMatricula
            }).ToList(),
            AlunosDisponiveis = alunosDisponiveis.Select(a => new EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao.SelectAlunoViewModel { AlunoId = a.Id, Nome = a.nome }).ToList()
        };

        return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/VerTurma.cshtml", vm);
    }

    [HttpPost]
    public ActionResult CadastrarMatricula(CadastrarMatriculaDto dto)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("VerTurma", new { id = dto.TurmaId });

        var resultado = servicoMatricula.CadastrarMatricula(dto);
        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction("VerTurma", new { id = dto.TurmaId });
    }

    [HttpPost]
    public ActionResult TrancarMatricula(Guid matriculaId, Guid turmaId)
    {
        var dto = new AlterarSituacaoMatriculaDto { MatriculaId = matriculaId, NovaSituacao = Modulos.ModuloMatricula.Dominio.SituacaoMatricula.Trancado };
        var resultado = servicoMatricula.AlterarSituacaoMatricula(dto);
        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction("VerTurma", new { id = turmaId });
    }

    [HttpGet]
    public ActionResult VerNotas(Guid matriculaId, Guid turmaId)
    {
        var ficha = servicoMatricula.ObterFichaNotas(matriculaId);
        if (ficha == null)
            return RedirectToAction("VerTurma", new { id = turmaId });

        ViewBag.TurmaId = turmaId;
        return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/VerNotas.cshtml", ficha);
    }

    [HttpPost]
    public ActionResult AtualizarNotas(AtualizarNotasDto dto, Guid turmaId)
    {
        if (dto.MatriculaId == Guid.Empty && Request.Form.TryGetValue("MatriculaId", out var matriculaIdRaw) &&
            Guid.TryParse(matriculaIdRaw, out var parsedMatriculaId))
        {
            dto.MatriculaId = parsedMatriculaId;
        }

        if (!TryParseNota(Request.Form, "Nota1", out var nota1) ||
            !TryParseNota(Request.Form, "Nota2", out var nota2) ||
            !TryParseNota(Request.Form, "Nota3", out var nota3) ||
            !TryParseNota(Request.Form, "Recuperacao", out var recuperacao))
        {
            TempData["MensagemErro"] = "Formato inválido nas notas. Use 7,50 ou 7.50.";
            return RedirectToAction("VerNotas", new { matriculaId = dto.MatriculaId, turmaId });
        }

        dto.Nota1 = nota1;
        dto.Nota2 = nota2;
        dto.Nota3 = nota3;
        dto.Recuperacao = recuperacao;

        ModelState.Remove(nameof(dto.Nota1));
        ModelState.Remove(nameof(dto.Nota2));
        ModelState.Remove(nameof(dto.Nota3));
        ModelState.Remove(nameof(dto.Recuperacao));

        if (!ModelState.IsValid)
            return RedirectToAction("VerNotas", new { matriculaId = dto.MatriculaId, turmaId });

        var resultado = servicoMatricula.AtualizarNotas(dto);
        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction("VerTurma", new { id = turmaId });
    }

    private static bool TryParseNota(Microsoft.AspNetCore.Http.IFormCollection form, string fieldName, out double? valor)
    {
        valor = null;

        if (!form.TryGetValue(fieldName, out var rawValue) || string.IsNullOrWhiteSpace(rawValue))
            return true;

        var normalized = rawValue.ToString().Trim().Replace(',', '.');

        if (double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
        {
            valor = parsed;
            return true;
        }

        return false;
    }

    [HttpGet]
    public ActionResult CadastrarTurma()
    {
        ViewBag.Cursos = repositorioCurso.SelecionarTodos().OrderBy(c => c.nome).ToList();
        ViewBag.Professores = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Professor && u.ativo)
            .OrderBy(u => u.nome)
            .ToList();

        return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/Cadastrar.cshtml", new CadastrarTurmaDto());
    }

    [HttpPost]
    public ActionResult CadastrarTurma(CadastrarTurmaDto cadastrarVm)
    {
        ViewBag.Cursos = repositorioCurso.SelecionarTodos().OrderBy(c => c.nome).ToList();
        ViewBag.Professores = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Professor && u.ativo)
            .OrderBy(u => u.nome)
            .ToList();

        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/Cadastrar.cshtml", cadastrarVm);

        var resultado = servicoTurma.CadastrarTurma(cadastrarVm);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors.First().Message);
            return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/Cadastrar.cshtml", cadastrarVm);
        }

        return RedirectToAction("ListarTurmas");
    }

    [HttpGet]
    public ActionResult EditarTurma(Guid id)
    {
        var turma = repositorioTurma.SelecionarPorId(id);

        if (turma == null)
            return NotFound();

        ViewBag.Cursos = repositorioCurso.SelecionarTodos().OrderBy(c => c.nome).ToList();
        ViewBag.Professores = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Professor &&
                (u.ativo || u.Id == turma.instrutorId))
            .OrderBy(u => u.nome)
            .ToList();

        var editarVm = new EditarTurmaDto
        {
            Id = turma.Id,
            nome = turma.nome,
            dataInicio = turma.dataInicio,
            dataFim = turma.dataFim,
            vagasMaximas = turma.vagasMaximas,
            HorarioTurno = turma.HorarioTurno,
            status = turma.status,
            cursoId = turma.cursoId,
            instrutorId = turma.instrutorId
        };

        return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/Editar.cshtml", editarVm);
    }

    [HttpPost]
    public ActionResult EditarTurma(Guid id, EditarTurmaDto editarVm)
    {
        ViewBag.Cursos = repositorioCurso.SelecionarTodos().OrderBy(c => c.nome).ToList();
        ViewBag.Professores = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Professor &&
                (u.ativo || u.Id == editarVm.instrutorId))
            .OrderBy(u => u.nome)
            .ToList();

        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/Editar.cshtml", editarVm);

        var resultado = servicoTurma.EditarTurma(id, editarVm);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors.First().Message);
            return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/Editar.cshtml", editarVm);
        }

        return RedirectToAction("ListarTurmas");
    }

    [HttpGet]
    public ActionResult ExcluirTurma(Guid id)
    {
        var turma = repositorioTurma.SelecionarPorId(id);

        if (turma == null)
            return NotFound();

        var curso = repositorioCurso.SelecionarPorId(turma.cursoId);
        var instrutor = repositorioUsuario.SelecionarPorId(turma.instrutorId);

        var excluirVm = new ExcluirTurmaDto
        {
            Id = turma.Id,
            nome = turma.nome,
            dataInicio = turma.dataInicio,
            dataFim = turma.dataFim,
            vagasMaximas = turma.vagasMaximas,
            HorarioTurno = turma.HorarioTurno,
            status = turma.status,
            cursoId = turma.cursoId,
            instrutorId = turma.instrutorId,
            CursoNome = curso?.nome ?? "-",
            InstrutorNome = instrutor?.nome ?? "-"
        };

        return View("~/Modulos/ModuloADM/Apresentacao/Views/TurmaADM/Excluir.cshtml", excluirVm);
    }

    [HttpPost]
    public ActionResult ExcluirTurma(Guid id, bool confirmado = true)
    {
        if (!confirmado)
            return RedirectToAction("ListarTurmas");

        if (!servicoTurma.ExcluirTurma(id))
            TempData["MensagemErro"] = "Falha ao excluir a turma.";

        return RedirectToAction("ListarTurmas");
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
                Telefone = u.telefone,
                Ativo = u.ativo
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

        var dto = new CadastrarAlunoDto(
            cadastrarVm.Nome,
            cadastrarVm.Email,
            cadastrarVm.Senha,
            cadastrarVm.Telefone
        );

        var resultado = servicoAluno.Cadastrar(dto);

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
        )
        {
            ativo = usuario.ativo
        };

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

        if (!servicoUsuario.DesativarUsuario(id, TipoUsuario.Aluno))
            TempData["MensagemErro"] = "Falha ao desativar o aluno.";

        return RedirectToAction("ListarAlunos");
    }

    [HttpGet]
    public ActionResult ListarProfessores()
    {
        var professores = repositorioUsuario.SelecionarTodos()
            .Where(u => u.tipoUsuario == TipoUsuario.Professor)
            .Select(usuario =>
            {
                var dados = servicoProfessor.SelecionarDadosParaPerfil(usuario.Id);

                return new ProfessorADMPerfilViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.nome,
                    Email = usuario.email,
                    Telefone = usuario.telefone,
                    Bio = dados?.Bio ?? string.Empty,
                    Especialidades = dados?.Especialidades ?? string.Empty,
                    DataContratacao = dados?.DataContratacao ?? DateTime.Today,
                    PerfilCadastrado = servicoProfessor.SelecionarPorId(usuario.Id) != null,
                    Ativo = usuario.ativo
                };
            })
            .ToList();

        return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Listar.cshtml", professores);
    }

    [HttpPost]
    public ActionResult SalvarPerfilProfessor(
        Guid id,
        string bio,
        string especialidades,
        DateTime dataContratacao)
    {
        var usuario = repositorioUsuario.SelecionarPorId(id);

        if (usuario == null || usuario.tipoUsuario != TipoUsuario.Professor)
            return NotFound();

        var resultado = servicoProfessor.SelecionarPorId(id) == null
            ? servicoProfessor.Cadastrar(new CadastrarProfessorPerfilDto(
                id,
                bio,
                especialidades,
                dataContratacao))
            : servicoProfessor.Editar(new EditarProfessorPerfilDto(
                id,
                bio,
                especialidades,
                dataContratacao));

        if (resultado.IsFailed)
        {
            TempData["MensagemPerfilErro"] = resultado.Errors.First().Message;
            return RedirectToAction(nameof(ListarProfessores), new { abrirPerfil = id });
        }

        TempData["MensagemPerfilSucesso"] = "Perfil profissional atualizado.";
        return RedirectToAction(nameof(ListarProfessores), new { abrirPerfil = id });
    }

    [HttpGet]
    public ActionResult CadastrarProfessor()
    {
        return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Cadastrar.cshtml", new CadastrarProfessorADMViewModel());
    }

    [HttpPost]
    public ActionResult CadastrarProfessor(CadastrarProfessorADMViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View("~/Modulos/ModuloADM/Apresentacao/Views/ProfessorADM/Cadastrar.cshtml", cadastrarVm);

        var dto = new CadastrarProfessorDto(
            cadastrarVm.Nome,
            cadastrarVm.Email,
            cadastrarVm.Senha,
            cadastrarVm.Telefone,
            cadastrarVm.Bio,
            cadastrarVm.Especialidades,
            cadastrarVm.DataContratacao
        );

        var resultado = servicoProfessor.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                string campo = erro.Metadata.TryGetValue("Campo", out object? valor)
                    ? valor?.ToString() ?? string.Empty
                    : string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

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
        )
        {
            ativo = usuario.ativo
        };

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

        if (!servicoUsuario.DesativarUsuario(
            id,
            TipoUsuario.Professor))
            TempData["MensagemErro"] = "Falha ao desativar o professor.";

        return RedirectToAction("ListarProfessores");
    }
}

