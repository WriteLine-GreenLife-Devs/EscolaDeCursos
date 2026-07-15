using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAluno;

public sealed class NotaAluno : EntidadeBase<NotaAluno>
{
    public Guid AlunoId { get; set; }
    public Guid MatriculaId { get; set; }
    public TipoNotaAluno TipoNota { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public double Valor { get; set; }
    public DateTime DataLancamento { get; set; }
    public Aluno Aluno { get; set; } = null!;

    public NotaAluno() { }

    public NotaAluno(
        Guid alunoId,
        Guid matriculaId,
        TipoNotaAluno tipoNota,
        string descricao,
        double valor,
        DateTime dataLancamento)
    {
        AlunoId = alunoId;
        MatriculaId = matriculaId;
        TipoNota = tipoNota;
        Descricao = descricao?.Trim() ?? string.Empty;
        Valor = Math.Round(valor, 2);
        DataLancamento = dataLancamento.Date;
    }

    public override void Atualizar(NotaAluno entidadeAtualizada)
    {
        TipoNota = entidadeAtualizada.TipoNota;
        Descricao = entidadeAtualizada.Descricao?.Trim() ?? string.Empty;
        Valor = Math.Round(entidadeAtualizada.Valor, 2);
        DataLancamento = entidadeAtualizada.DataLancamento.Date;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (AlunoId == Guid.Empty)
            erros.Add("O aluno da nota é obrigatório.");

        if (MatriculaId == Guid.Empty)
            erros.Add("A matrícula da nota é obrigatória.");

        if (!Enum.IsDefined(TipoNota) ||
            TipoNota == TipoNotaAluno.Indefinida)
        {
            erros.Add("O tipo da nota é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(Descricao))
        {
            erros.Add("O campo \"Descrição\" deve ser preenchido.");
        }
        else if (Descricao.Length > 100)
        {
            erros.Add(
                "O campo \"Descrição\" deve conter no máximo 100 caracteres."
            );
        }

        if (double.IsNaN(Valor) ||
            double.IsInfinity(Valor) ||
            Valor < 0 ||
            Valor > 10)
        {
            erros.Add("O campo \"Nota\" deve estar entre 0 e 10.");
        }

        if (DataLancamento == default)
        {
            erros.Add("A data de lançamento da nota é obrigatória.");
        }
        else if (DataLancamento.Date > DateTime.Today)
        {
            erros.Add("A data de lançamento da nota não pode ser futura.");
        }

        return erros;
    }
}
