using System.Security.Claims;
using AutoMapper;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Apresentacao;

[Authorize(Roles = "Aluno")]
[Route("ModuloAluno/Apresentacao/[action]")]
public sealed class AlunoController(
    ServicoAluno servicoAluno,
    ServicoNotaAluno servicoNotaAluno,
    ServicoPresencaAluno servicoPresencaAluno,
    IMapper mapeador
) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        if (!TentarObterAlunoId(out Guid alunoId))
            return Forbid();

        DetalhesAlunoDto? alunoDto = servicoAluno.SelecionarPorId(alunoId);

        if (alunoDto == null)
            return NotFound();

        var viewModel = new PainelAlunoViewModel
        {
            Aluno = mapeador.Map<DetalhesAlunoViewModel>(alunoDto),
            Matriculas = mapeador.Map<List<MatriculaPainelAlunoViewModel>>(
                servicoAluno.SelecionarMatriculas(alunoId))
        };

        return View(viewModel);
    }

    [HttpGet]
    public ActionResult DetalhesMatricula(Guid id)
    {
        if (!TentarObterAlunoId(out Guid alunoId))
            return Forbid();

        MatriculaPainelAlunoDto? matriculaDto =
            servicoAluno.SelecionarMatricula(alunoId, id);

        if (matriculaDto == null)
            return NotFound();

        var viewModel = new DetalhesMatriculaAlunoViewModel
        {
            Matricula = mapeador.Map<MatriculaPainelAlunoViewModel>(
                matriculaDto),
            Notas = mapeador.Map<List<NotaAlunoViewModel>>(
                servicoNotaAluno.SelecionarPorMatricula(id)),
            Presencas = mapeador.Map<List<PresencaAlunoViewModel>>(
                servicoPresencaAluno.SelecionarPorMatricula(id))
        };

        return View(viewModel);
    }

    private bool TentarObterAlunoId(out Guid alunoId)
    {
        string? identificador = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        return Guid.TryParse(identificador, out alunoId);
    }
}