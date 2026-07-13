using EscolaDeCursosWebApp.Compartilhado.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;

public sealed class Matricula : EntidadeBase<Matricula>
{
    public Guid AlunoId { get; set; }
    public Guid TurmaId { get; set; }
    public DateTime DataMatricula { get; set; } = DateTime.Now;
    public SituacaoMatricula Situacao { get; set; } = SituacaoMatricula.Cursando;

    public Matricula()
    {
    }
    public Matricula(Guid alunoId, Guid turmaId, DateTime dataMatricula, SituacaoMatricula situacao)
    {
        AlunoId = alunoId;
        TurmaId = turmaId;
        DataMatricula = dataMatricula;
        Situacao = situacao;
    }
    public override void Atualizar(Matricula entidadeAtualizada)
    {
        Situacao = entidadeAtualizada.Situacao;
    }

    public override List<string> Validar()
    {
        var erros = new List<string>();

        if (AlunoId == Guid.Empty)
            erros.Add("O ID do aluno é obrigatório.");

        if (TurmaId == Guid.Empty)
            erros.Add("O ID da turma é obrigatório.");

        return erros;
    }
}
