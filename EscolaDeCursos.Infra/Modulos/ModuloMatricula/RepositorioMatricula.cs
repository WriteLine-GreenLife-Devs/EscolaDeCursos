using System.Data;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursos.Infra.Modulos.ModuloMatricula;

public sealed class RepositorioMatricula :
    RepositorioBase<Matricula>, IRepositorioMatricula
{
    private readonly EscolaDeCursosDbContext contexto;

    public RepositorioMatricula(EscolaDeCursosDbContext contexto)
        : base(contexto)
    {
        this.contexto = contexto;
    }

    public ResultadoCadastroMatricula CadastrarComControleDeVagas(
        Matricula matricula,
        int vagasMaximas)
    {
        var estrategiaExecucao = contexto.Database.CreateExecutionStrategy();

        return estrategiaExecucao.Execute(() =>
            CadastrarEmTransacao(matricula, vagasMaximas));
    }

    private ResultadoCadastroMatricula CadastrarEmTransacao(
        Matricula matricula,
        int vagasMaximas)
    {
        using var transacao = contexto.Database.BeginTransaction(
            IsolationLevel.Serializable);

        contexto.Database.ExecuteSqlInterpolated($"""
            SELECT 1
            FROM [TBTurma] WITH (UPDLOCK, HOLDLOCK)
            WHERE [Id] = {matricula.TurmaId}
            """);

        bool matriculaExistente = registros.Any(registro =>
            registro.AlunoId == matricula.AlunoId &&
            registro.TurmaId == matricula.TurmaId);

        if (matriculaExistente)
            return ResultadoCadastroMatricula.MatriculaExistente;

        int vagasOcupadas = registros.Count(registro =>
            registro.TurmaId == matricula.TurmaId &&
            registro.Situacao == SituacaoMatricula.Cursando);

        if (vagasOcupadas >= vagasMaximas)
            return ResultadoCadastroMatricula.SemVagas;

        try
        {
            registros.Add(matricula);
            contexto.SaveChanges();
            transacao.Commit();

            return ResultadoCadastroMatricula.Sucesso;
        }
        catch (DbUpdateException excecao)
            when (excecao.InnerException is SqlException
            {
                Number: 2601 or 2627
            })
        {
            transacao.Rollback();
            contexto.Entry(matricula).State = EntityState.Detached;

            return ResultadoCadastroMatricula.MatriculaExistente;
        }
    }

    public override List<Matricula> SelecionarTodos()
    {
        return registros.OrderBy(m => m.DataMatricula).ToList();
    }

    public override List<Matricula> Filtrar(Func<Matricula, bool> filtro)
    {
        return registros.Where(filtro).OrderBy(m => m.DataMatricula).ToList();
    }
}
