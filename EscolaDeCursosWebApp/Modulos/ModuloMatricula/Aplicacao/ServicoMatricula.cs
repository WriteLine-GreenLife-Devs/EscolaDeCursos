using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;

public sealed class ServicoMatricula : ServicoBase<Matricula>
{
    private readonly IRepositorioMatricula repositorioMatricula;
    private readonly IRepositorioTurma repositorioTurma;
    private readonly IRepositorioUsuario repositorioUsuario;

    public ServicoMatricula(
        IRepositorioMatricula repositorioMatricula,
        IRepositorioTurma repositorioTurma,
        IRepositorioUsuario repositorioUsuario)
    {
        this.repositorioMatricula = repositorioMatricula;
        this.repositorioTurma = repositorioTurma;
        this.repositorioUsuario = repositorioUsuario;
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    public Result CadastrarMatricula(CadastrarMatriculaDto dto)
    {
        var turma = repositorioTurma.SelecionarPorId(dto.TurmaId);
        if (turma == null)
            return Falha(nameof(dto.TurmaId), "Turma não encontrada.");

        var aluno = repositorioUsuario.SelecionarPorId(dto.AlunoId);
        if (aluno == null || aluno.tipoUsuario != TipoUsuario.Aluno)
            return Falha(nameof(dto.AlunoId), "Aluno não encontrado ou inválido.");

        // contar ocupadas (apenas Cursando ocupam vaga)
        var ocupadas = repositorioMatricula.Filtrar(m => m.TurmaId == dto.TurmaId && m.Situacao == SituacaoMatricula.Cursando).Count;
        if (ocupadas >= turma.vagasMaximas)
            return Falha(string.Empty, "Não há vagas disponíveis nesta turma.");

        // evitar matricular o mesmo aluno duas vezes como Cursando
        var existenteAtiva = repositorioMatricula.Filtrar(m => m.TurmaId == dto.TurmaId && m.AlunoId == dto.AlunoId && m.Situacao == SituacaoMatricula.Cursando).Any();
        if (existenteAtiva)
            return Falha(string.Empty, "Aluno já está matriculado nesta turma.");

        var matricula = new Matricula(dto.AlunoId, dto.TurmaId, DateTime.Now, SituacaoMatricula.Cursando);

        var erros = matricula.Validar();
        if (erros.Count > 0)
            return Falha(string.Empty, erros.First());

        repositorioMatricula.Cadastrar(matricula);
        return Result.Ok();
    }

    public Result AlterarSituacaoMatricula(AlterarSituacaoMatriculaDto dto)
    {
        var matricula = repositorioMatricula.SelecionarPorId(dto.MatriculaId);
        if (matricula == null)
            return Falha(nameof(dto.MatriculaId), "Matrícula não encontrada.");

        if (matricula.Situacao == SituacaoMatricula.Trancado)
            return Falha(string.Empty, "Matrícula trancada não pode ser alterada.");

        // Permitimos apenas trancar a matrícula através desta ação (conforme regra atual)
        if (dto.NovaSituacao != SituacaoMatricula.Trancado)
            return Falha(nameof(dto.NovaSituacao), "Só é permitido alterar a situação para 'Trancado' via esta ação.");

        matricula.Situacao = SituacaoMatricula.Trancado;

        if (!repositorioMatricula.Editar(dto.MatriculaId, matricula))
            return Falha(string.Empty, "Falha ao atualizar situação da matrícula.");

        return Result.Ok();
    }

    public TurmaDetalheDto? ObterDetalhesTurma(Guid turmaId)
    {
        var turma = repositorioTurma.SelecionarPorId(turmaId);
        if (turma == null) return null;

        var instrutor = repositorioUsuario.SelecionarPorId(turma.instrutorId);

        var matriculas = repositorioMatricula.Filtrar(m => m.TurmaId == turmaId)
            .Select(m => new MatriculaAlunoDto
            {
                MatriculaId = m.Id,
                AlunoId = m.AlunoId,
                AlunoNome = (repositorioUsuario.SelecionarPorId(m.AlunoId)?.nome) ?? string.Empty,
                Situacao = m.Situacao,
                DataMatricula = m.DataMatricula
            })
            .OrderBy(m => m.DataMatricula)
            .ToList();

        return new TurmaDetalheDto
        {
            TurmaId = turma.Id,
            TurmaNome = turma.nome,
            InstrutorId = turma.instrutorId,
            InstrutorNome = instrutor?.nome ?? string.Empty,
            VagasMaximas = turma.vagasMaximas,
            Matriculas = matriculas
        };
    }
}
