using System.ComponentModel.DataAnnotations;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;

namespace EscolaDeCursosWebApp.Modulos.ModuloUsuario.Apresentacao;

public class CadastrarUsuarioViewModel
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;

    [Required]
    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;

    [Required]
    public string Telefone { get; set; } = string.Empty;

    public TipoUsuario TipoUsuario { get; set; } = TipoUsuario.Aluno;
}
