using EscolaDeCursosWebApp.Compartilhado.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Dominio;

public sealed class ProfessorPerfil : EntidadeBase<ProfessorPerfil>
{
    public string bio { get; set; } = string.Empty;
    public string especialidades { get; set; } = string.Empty;
    public DateTime dataContratacao { get; set; } = DateTime.Today;

    public Usuario Usuario { get; set; } = null!;

    public ProfessorPerfil()
    {
    }

    public ProfessorPerfil(
        Guid usuarioId,
        string bio,
        string especialidades,
        DateTime dataContratacao)
    {
        Id = usuarioId;
        this.bio = bio;
        this.especialidades = especialidades;
        this.dataContratacao = dataContratacao;
    }

    public override void Atualizar(ProfessorPerfil entidadeAtualizada)
    {
        bio = entidadeAtualizada.bio;
        especialidades = entidadeAtualizada.especialidades;
        dataContratacao = entidadeAtualizada.dataContratacao;
    }

    public override List<string> Validar()
    {
        var erros = new List<string>();

        if (Id == Guid.Empty)
            erros.Add("O professor deve estar vinculado a um usuário.");

        if (string.IsNullOrWhiteSpace(bio))
            erros.Add("O campo \"Biografia\" deve ser preenchido.");
        else if (bio.Length > 1000)
            erros.Add("O campo \"Biografia\" deve conter no máximo 1000 caracteres.");

        if (string.IsNullOrWhiteSpace(especialidades))
            erros.Add("O campo \"Especialidades\" deve ser preenchido.");
        else if (especialidades.Length > 500)
            erros.Add("O campo \"Especialidades\" deve conter no máximo 500 caracteres.");

        if (dataContratacao == default)
            erros.Add("O campo \"Data de contratação\" deve ser preenchido.");
        else if (dataContratacao.Date > DateTime.Today)
            erros.Add("A data de contratação não pode ser futura.");

        return erros;
    }
}