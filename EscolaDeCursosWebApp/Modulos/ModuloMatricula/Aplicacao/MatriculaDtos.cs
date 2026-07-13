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
