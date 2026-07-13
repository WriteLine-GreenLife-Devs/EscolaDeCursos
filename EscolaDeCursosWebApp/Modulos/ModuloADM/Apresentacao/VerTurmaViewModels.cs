using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

public sealed class VerTurmaViewModel
{
    public Guid TurmaId { get; set; }
    public string TurmaNome { get; set; } = string.Empty;
    public string InstrutorNome { get; set; } = string.Empty;
    public int VagasMaximas { get; set; }
    public List<AlunoMatriculaViewModel> Matriculas { get; set; } = new();
    public List<SelectAlunoViewModel> AlunosDisponiveis { get; set; } = new();
}

public sealed class AlunoMatriculaViewModel
{
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public SituacaoMatricula Situacao { get; set; }
    public DateTime DataMatricula { get; set; }
}

public sealed class SelectAlunoViewModel
{
    public Guid AlunoId { get; set; }
    public string Nome { get; set; } = string.Empty;
}
