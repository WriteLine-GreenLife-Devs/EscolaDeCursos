using System.ComponentModel.DataAnnotations;
using EscolaDeCursosWebApp.Modulos.ModuloMatricula.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloMatricula.Aplicacao;

public sealed class CadastrarMatriculaDto
{
    [Required(ErrorMessage = "A turma é obrigatória.")]
    public Guid TurmaId { get; set; }

    [Required(ErrorMessage = "O aluno é obrigatório.")]
    public Guid AlunoId { get; set; }
}

public sealed class AlterarSituacaoMatriculaDto
{
    [Required]
    public Guid MatriculaId { get; set; }

    [Required]
    public SituacaoMatricula NovaSituacao { get; set; }
}

public sealed class MatriculaAlunoDto
{
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public SituacaoMatricula Situacao { get; set; }
    public DateTime DataMatricula { get; set; }
}

public sealed class TurmaDetalheDto
{
    public Guid TurmaId { get; set; }
    public string TurmaNome { get; set; } = string.Empty;
    public Guid InstrutorId { get; set; }
    public string InstrutorNome { get; set; } = string.Empty;
    public int VagasMaximas { get; set; }
    public List<MatriculaAlunoDto> Matriculas { get; set; } = new();
}
