using EscolaDeCursos.Dominio.Modulos.ModuloTurma;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

public class TurmaADMViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int VagasMaximas { get; set; }
    public string HorarioTurno { get; set; } = string.Empty;
    public StatusTurma Status { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public string InstrutorNome { get; set; } = string.Empty;
}
