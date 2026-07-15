using System.ComponentModel.DataAnnotations;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloMatricula;

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

public sealed class AtualizarNotasDto
{
    [Required]
    public Guid MatriculaId { get; set; }

    public double? Nota1 { get; set; }
    public double? Nota2 { get; set; }
    public double? Nota3 { get; set; }
    public double? Recuperacao { get; set; }
}

public sealed class FichaNotasDto
{
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public double? Nota1 { get; set; }
    public double? Nota2 { get; set; }
    public double? Nota3 { get; set; }
    public double? Recuperacao { get; set; }
    public double? NotaFinal { get; set; }
    public SituacaoMatricula Situacao { get; set; }
}
