using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

public sealed class VerTurmaViewModel
{
    public Guid TurmaId { get; set; }
    public string TurmaNome { get; set; } = string.Empty;
    public string InstrutorNome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int VagasMaximas { get; set; }
    public List<AlunoMatriculaViewModel> Matriculas { get; set; } = new();
    public List<SelectAlunoViewModel> AlunosDisponiveis { get; set; } = new();
    public List<ChamadaADMViewModel> Chamadas { get; set; } = new();
}

public sealed class AlunoMatriculaViewModel
{
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public SituacaoMatricula Situacao { get; set; }
    public DateTime DataMatricula { get; set; }
    public double? NotaFinal { get; set; }
    public int FrequenciaPercentual { get; set; }
    public int TotalPresencas { get; set; }
}

public sealed class SelectAlunoViewModel
{
    public Guid AlunoId { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public sealed record ChamadaADMViewModel(
    DateTime Data,
    List<AlunoChamadaADMViewModel> Alunos
);

public sealed record AlunoChamadaADMViewModel(
    Guid MatriculaId,
    string Nome,
    bool? Presente
);

public sealed class SalvarChamadaADMViewModel
{
    public Guid TurmaId { get; set; }
    public DateTime DataAula { get; set; }
    public List<PresencaChamadaADMViewModel> Alunos { get; set; } = [];
}

public sealed class PresencaChamadaADMViewModel
{
    public Guid MatriculaId { get; set; }
    public bool Presente { get; set; }
}