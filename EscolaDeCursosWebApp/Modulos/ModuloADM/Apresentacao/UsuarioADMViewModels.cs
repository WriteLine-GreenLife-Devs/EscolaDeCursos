using System.ComponentModel.DataAnnotations;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

public class UsuarioADMViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}

public class CadastrarUsuarioADMViewModel
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Telefone { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;

    [Required]
    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;
}

public sealed class CadastrarProfessorADMViewModel : CadastrarUsuarioADMViewModel
{
    [Required]
    [StringLength(1000)]
    [Display(Name = "Biografia")]
    public string Bio { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Especialidades { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Data de contratação")]
    public DateTime DataContratacao { get; set; } = DateTime.Today;
}

public class EditarUsuarioADMViewModel
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Telefone { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;
}

public class ExcluirUsuarioADMViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}
