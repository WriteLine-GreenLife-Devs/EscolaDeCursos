using System.Security.Claims;
using AutoMapper;
using EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EscolaDeCursos.Aplicacao.Modulos.ModuloConteudoCurso;
using EscolaDeCursos.Aplicacao.Modulos.ModuloProfessor;
using EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;
using EscolaDeCursos.Aplicacao.Modulos.ModuloMatricula;
using EscolaDeCursos.Aplicacao.Modulos.ModuloTurma;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Apresentacao;

[Authorize(Roles = "Professor")]
[Route("ModuloProfessor/Apresentacao/[action]")]
public sealed class ProfessorController(
    ServicoTurma servicoTurma,
    ServicoMatricula servicoMatricula,
    ServicoProfessor servicoProfessor,
    ServicoNotaAluno servicoNotaAluno,
    ServicoAluno servicoAluno,
    ServicoPresencaAluno servicoPresencaAluno,
    ServicoModuloCurso servicoModuloCurso,
    ServicoProgressoModuloAluno servicoProgressoModuloAluno,
    IMapper mapeador
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
                turma.CursoId,
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
            Cursos = turmas
                .GroupBy(turma => turma.CursoNome)
                .Select(grupo => new CursoProfessorViewModel(
                    grupo.Key,
                    grupo.Sum(turma => turma.TotalAlunos),
                    grupo.ToList()))
                .OrderBy(curso => curso.Nome)
                .ToList()
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

        TurmaProfessorViewModel? turmaResumo = servicoTurma
            .SelecionarPorProfessor(professorId)
            .Where(turmaProfessor => turmaProfessor.Id == id)
            .Select(turmaProfessor => new TurmaProfessorViewModel(
                turmaProfessor.Id,
                turmaProfessor.CursoId,
                turmaProfessor.Nome,
                turmaProfessor.CursoNome,
                turmaProfessor.DataInicio,
                turmaProfessor.DataFim,
                turmaProfessor.HorarioTurno,
                turmaProfessor.Status,
                turmaProfessor.TotalAlunos))
            .SingleOrDefault();

        if (turmaResumo == null)
            return NotFound();

        List<AlunoTurmaProfessorViewModel> alunos = turma.Matriculas
            .Select(matricula =>
            {
                FichaNotasAlunoDto? ficha = servicoNotaAluno
                    .SelecionarFicha(matricula.MatriculaId);

                DetalhesAlunoDto? aluno = servicoAluno
                    .SelecionarPorId(matricula.AlunoId);

                List<MatriculaAlunoProfessorViewModel> matriculas = servicoAluno
                    .SelecionarMatriculas(matricula.AlunoId)
                    .Select(matriculaAluno =>
                    {
                        List<PresencaAlunoDto> presencas = servicoPresencaAluno
                            .SelecionarPorMatricula(matriculaAluno.Id);

                        int totalPresentes = presencas.Count(presenca =>
                            presenca.Presente);

                        int frequenciaPercentual = presencas.Count == 0
                            ? 0
                            : (int)Math.Round(
                                totalPresentes * 100d / presencas.Count);

                        return new MatriculaAlunoProfessorViewModel(
                            matriculaAluno.Id,
                            matriculaAluno.Curso.Nome,
                            matriculaAluno.Turma.Nome,
                            matriculaAluno.Situacao,
                            frequenciaPercentual,
                            presencas.Count,
                            totalPresentes,
                            presencas
                                .Select(presenca =>
                                    new PresencaAlunoProfessorViewModel(
                                        presenca.DataAula,
                                        presenca.Presente))
                                .OrderByDescending(presenca => presenca.Data)
                                .ToList());
                    })
                    .OrderBy(matriculaAluno => matriculaAluno.CursoNome)
                    .ThenBy(matriculaAluno => matriculaAluno.TurmaNome)
                    .ToList();

                return new AlunoTurmaProfessorViewModel(
                    matricula.MatriculaId,
                    matricula.AlunoId,
                    matricula.AlunoNome,
                    aluno?.Email ?? string.Empty,
                    aluno?.Telefone ?? string.Empty,
                    aluno?.Ativo ?? false,
                    matricula.DataMatricula,
                    ficha?.Situacao ?? matricula.Situacao,
                    ficha?.Nota1,
                    ficha?.Nota2,
                    ficha?.Nota3,
                    ficha?.Recuperacao,
                    ficha?.NotaFinal,
                    mapeador.Map<ResumoProgressoModuloAlunoViewModel>(
                        servicoProgressoModuloAluno
                            .SelecionarProgressoDaMatricula(
                                matricula.MatriculaId)),
                    matriculas);
            })
            .OrderBy(aluno => aluno.Nome)
            .ToList();

        List<DateTime> datasChamadas = alunos
            .SelectMany(aluno => aluno.Matriculas
                .Where(matricula => matricula.Id == aluno.MatriculaId)
                .SelectMany(matricula => matricula.Presencas))
            .Select(presenca => presenca.Data.Date)
            .Distinct()
            .OrderByDescending(data => data)
            .ToList();

        var viewModel = new DetalhesTurmaProfessorViewModel
        {
            TurmaId = turma.TurmaId,
            TurmaNome = turma.TurmaNome,
            DataInicio = turmaResumo.DataInicio,
            DataFim = turmaResumo.DataFim,
            VagasMaximas = turma.VagasMaximas,
            ModulosCurso = new ModulosCursoParcialViewModel
            {
                CursoId = turmaResumo.CursoId,
                PermiteEdicao = false,
                Modulos = mapeador.Map<List<ModuloCursoViewModel>>(
                    servicoModuloCurso.SelecionarPorCurso(
                        turmaResumo.CursoId))
            },
            Alunos = alunos,
            Chamadas = datasChamadas
                .Select(data => new ChamadaProfessorViewModel(
                    data,
                    alunos
                        .Where(aluno => aluno.DataMatricula.Date <= data)
                        .Select(aluno =>
                        {
                            PresencaAlunoProfessorViewModel? presenca = aluno
                                .Matriculas
                                .Single(matricula =>
                                    matricula.Id == aluno.MatriculaId)
                                .Presencas
                                .SingleOrDefault(registro =>
                                    registro.Data.Date == data);

                            return new AlunoChamadaProfessorViewModel(
                                aluno.MatriculaId,
                                aluno.Nome,
                                presenca?.Presente);
                        })
                        .ToList()))
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult SalvarNotas(SalvarNotasProfessorViewModel viewModel)
    {
        if (!TentarObterProfessorId(out Guid professorId))
            return Forbid();

        if (!ModelState.IsValid)
        {
            TempData["MensagemNotasErro"] =
                "As notas devem estar entre 0 e 10.";

            return RedirectToAction(
                nameof(DetalhesTurma),
                new { id = viewModel.TurmaId });
        }

        Result resultado = servicoNotaAluno.SalvarNotasDoProfessor(
            new SalvarNotasAlunoDto(
                viewModel.MatriculaId,
                viewModel.Nota1,
                viewModel.Nota2,
                viewModel.Nota3,
                viewModel.Recuperacao),
            professorId);

        if (resultado.IsFailed)
        {
            TempData["MensagemNotasErro"] =
                resultado.Errors.First().Message;

            return RedirectToAction(
                nameof(DetalhesTurma),
                new { id = viewModel.TurmaId });
        }

        TempData["MensagemNotasSucesso"] =
            "Notas atualizadas com sucesso.";

        return RedirectToAction(
            nameof(DetalhesTurma),
            new { id = viewModel.TurmaId });
    }

    [HttpPost]
    public ActionResult SalvarChamada(SalvarChamadaProfessorViewModel viewModel)
    {
        if (!TentarObterProfessorId(out Guid professorId))
            return Forbid();

        if (!ModelState.IsValid)
        {
            TempData["MensagemChamadaErro"] =
                "Verifique os dados informados na chamada.";

            return RedirectToAction(
                nameof(DetalhesTurma),
                new { id = viewModel.TurmaId });
        }

        Result resultado = servicoPresencaAluno.SalvarChamadaDoProfessor(
            new SalvarChamadaAlunoDto(
                viewModel.TurmaId,
                viewModel.DataAula,
                viewModel.Alunos
                    .Select(aluno => new PresencaChamadaAlunoDto(
                        aluno.MatriculaId,
                        aluno.Presente))
                    .ToList()),
            professorId);

        if (resultado.IsFailed)
        {
            TempData["MensagemChamadaErro"] =
                resultado.Errors.First().Message;

            return RedirectToAction(
                nameof(DetalhesTurma),
                new { id = viewModel.TurmaId });
        }

        TempData["MensagemChamadaSucesso"] =
            "Chamada salva com sucesso.";

        return RedirectToAction(
            nameof(DetalhesTurma),
            new { id = viewModel.TurmaId });
    }

    private bool TentarObterProfessorId(out Guid professorId)
    {
        string? identificador = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(identificador, out professorId);
    }
}