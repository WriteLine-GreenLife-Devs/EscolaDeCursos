using EscolaDeCursos.Dominio.Modulos.ModuloCurso;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

public class CursoADMViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }
    public NivelDificuldade NivelDificuldade { get; set; }
    public StatusCurso Status { get; set; }
    public decimal Valor { get; set; }
    public string CategoriaNome { get; set; } = string.Empty;
}