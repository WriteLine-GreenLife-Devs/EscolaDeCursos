using System.ComponentModel.DataAnnotations;
using EscolaDeCursosWebApp.Modulos.ModuloTurma.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloTurma.Aplicacao;

public sealed class CadastrarTurmaDto
{
    public CadastrarTurmaDto()
    {
        dataInicio = DateTime.Today;
        dataFim = DateTime.Today.AddDays(1);
    }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string nome { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime dataInicio { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime dataFim { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O número de vagas máximas deve ser maior que zero.")]
    public int vagasMaximas { get; set; } = 1;

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string HorarioTurno { get; set; } = string.Empty;

    [Required]
    public StatusTurma status { get; set; } = StatusTurma.InscricoesAbertas;

    [Required(ErrorMessage = "O curso da turma é obrigatório.")]
    public Guid cursoId { get; set; }

    [Required(ErrorMessage = "O instrutor da turma é obrigatório.")]
    public Guid instrutorId { get; set; }
}

public sealed class EditarTurmaDto
{
    public EditarTurmaDto()
    {
        dataInicio = DateTime.Today;
        dataFim = DateTime.Today.AddDays(1);
    }

    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string nome { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime dataInicio { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime dataFim { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O número de vagas máximas deve ser maior que zero.")]
    public int vagasMaximas { get; set; } = 1;

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string HorarioTurno { get; set; } = string.Empty;

    [Required]
    public StatusTurma status { get; set; } = StatusTurma.InscricoesAbertas;

    [Required(ErrorMessage = "O curso da turma é obrigatório.")]
    public Guid cursoId { get; set; }

    [Required(ErrorMessage = "O instrutor da turma é obrigatório.")]
    public Guid instrutorId { get; set; }
}

public sealed class ExcluirTurmaDto
{
    public Guid Id { get; set; }
    public string nome { get; set; } = string.Empty;
    public DateTime dataInicio { get; set; } = DateTime.Today;
    public DateTime dataFim { get; set; } = DateTime.Today;
    public int vagasMaximas { get; set; } = 1;
    public string HorarioTurno { get; set; } = string.Empty;
    public StatusTurma status { get; set; } = StatusTurma.InscricoesAbertas;
    public Guid cursoId { get; set; }
    public Guid instrutorId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public string InstrutorNome { get; set; } = string.Empty;
}
