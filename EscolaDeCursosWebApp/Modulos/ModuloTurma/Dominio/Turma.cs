using EscolaDeCursosWebApp.Compartilhado.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;

public sealed class Turma : EntidadeBase<Turma>
{
    public string nome { get; set; } = string.Empty;
    public DateTime dataInicio { get; set; } = DateTime.Now;
    public DateTime dataFim { get; set; } = DateTime.Now;
    public int vagasMaximas { get; set; } = 0;
    public string HorarioTurno { get; set; } = string.Empty;
    public StatusTurma status { get; set; } = StatusTurma.InscricoesAbertas;
    public Guid cursoId { get; set; }
    public Guid instrutorId { get; set; }
    public override void Atualizar(Turma entidadeAtualizada)
    {
        this.nome = entidadeAtualizada.nome;
        this.dataInicio = entidadeAtualizada.dataInicio;
        this.dataFim = entidadeAtualizada.dataFim;
        this.vagasMaximas = entidadeAtualizada.vagasMaximas;
        this.HorarioTurno = entidadeAtualizada.HorarioTurno;
        this.status = entidadeAtualizada.status;
        this.cursoId = entidadeAtualizada.cursoId;
        this.instrutorId = entidadeAtualizada.instrutorId;
    }

    public override List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(nome))
            erros.Add("O nome da turma é obrigatório.");

        else if (nome.Length < 3 || nome.Length > 100)
            erros.Add("O nome da turma deve ter entre 3 e 100 caracteres.");

        if (dataInicio >= dataFim)
            erros.Add("A data de início deve ser anterior à data de término.");

        if (vagasMaximas <= 0)
            erros.Add("O número de vagas máximas deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(HorarioTurno))
            erros.Add("O horário/turno da turma é obrigatório.");

        return erros;
    }
}
