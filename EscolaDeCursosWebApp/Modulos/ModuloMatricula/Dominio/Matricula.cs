using EscolaDeCursosWebApp.Compartilhado.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;

public sealed class Matricula : EntidadeBase<Matricula>
{
    public Guid AlunoId { get; set; }
    public Guid TurmaId { get; set; }
    public DateTime DataMatricula { get; set; } = DateTime.Now;
    public SituacaoMatricula Situacao { get; set; } = SituacaoMatricula.Cursando;

    // notas
    public double? Nota1 { get; set; }
    public double? Nota2 { get; set; }
    public double? Nota3 { get; set; }
    public double? Recuperacao { get; set; }
    public double? NotaFinal { get; set; }

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
        // atualizar notas quando entidadeAtualizada vier com valores
        Nota1 = entidadeAtualizada.Nota1;
        Nota2 = entidadeAtualizada.Nota2;
        Nota3 = entidadeAtualizada.Nota3;
        Recuperacao = entidadeAtualizada.Recuperacao;
        NotaFinal = entidadeAtualizada.NotaFinal;
    }

    public void AtualizarNotas(double? nota1, double? nota2, double? nota3, double? recuperacao)
    {
        // usar FluentResults lightweight pattern: create Result-like struct? project uses FluentResults in services; here we'll return null/throw? Keep simple: return null if ok? But EntidadeBase doesn't reference FluentResults. We'll implement logic and throw exceptions for invalid operations.
        if (Situacao != SituacaoMatricula.Cursando)
            throw new InvalidOperationException("Não é possível editar notas de uma matrícula que não está em Cursando.");

        void ValidarNota(double? n)
        {
            if (n.HasValue && (n.Value < 0 || n.Value > 10))
                throw new ArgumentException("Notas devem estar entre 0 e 10.");
        }

        ValidarNota(nota1);
        ValidarNota(nota2);
        ValidarNota(nota3);
        ValidarNota(recuperacao);

        Nota1 = nota1.HasValue ? Math.Round(nota1.Value, 2) : null;
        Nota2 = nota2.HasValue ? Math.Round(nota2.Value, 2) : null;
        Nota3 = nota3.HasValue ? Math.Round(nota3.Value, 2) : null;
        Recuperacao = recuperacao.HasValue ? Math.Round(recuperacao.Value, 2) : null;

        // calcular média se as 3 notas estiverem presentes
        if (Nota1.HasValue && Nota2.HasValue && Nota3.HasValue)
        {
            double media = Math.Round((Nota1.Value + Nota2.Value + Nota3.Value) / 3.0, 2);

            if (media >= 6.0)
            {
                NotaFinal = media;
                Situacao = SituacaoMatricula.Concluido;
            }
            else if (Recuperacao.HasValue)
            {
                double final = Math.Round((media + Recuperacao.Value) / 2.0, 2);
                NotaFinal = final;
                Situacao = final >= 6.0 ? SituacaoMatricula.Concluido : SituacaoMatricula.Reprovado;
            }
            else
            {
                // mantém Cursando até que recuperação seja lançada
                NotaFinal = media;
                Situacao = SituacaoMatricula.Cursando;
            }
        }

        return;
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
