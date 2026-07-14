namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

public sealed class ProfessorADMPerfilViewModel
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public string Bio { get; init; } = string.Empty;
    public string Especialidades { get; init; } = string.Empty;
    public DateTime DataContratacao { get; init; } = DateTime.Today;
    public bool PerfilCadastrado { get; init; }
}
