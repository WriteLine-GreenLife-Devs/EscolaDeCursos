using EscolaDeCursosWebApp.Compartilhado.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Dominio;

public sealed class Professor : EntidadeBase<Professor>
{
    public string Bio { get; set; } = string.Empty;
    public string Especialidades { get; set; } = string.Empty;
    public DateTime DataContratacao { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public Professor() { }

    public Professor(
        Guid usuarioId,
        string bio,
        string especialidades,
        DateTime dataContratacao)
    {
        Id = usuarioId;
        Bio = bio?.Trim() ?? string.Empty;
        Especialidades = especialidades?.Trim() ?? string.Empty;
        DataContratacao = dataContratacao.Date;
    }

    public override void Atualizar(Professor entidadeAtualizada)
    {
        Bio = entidadeAtualizada.Bio?.Trim() ?? string.Empty;
        Especialidades =
            entidadeAtualizada.Especialidades?.Trim() ?? string.Empty;

        DataContratacao = entidadeAtualizada.DataContratacao.Date;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (Id == Guid.Empty)
            erros.Add(
                "O professor deve estar vinculado a um usuário."
            );

        if (string.IsNullOrWhiteSpace(Bio))
        {
            erros.Add(
                "O campo \"Biografia\" deve ser preenchido."
            );
        }
        else if (Bio.Length > 1000)
        {
            erros.Add(
                "O campo \"Biografia\" deve conter no máximo 1000 caracteres."
            );
        }

        if (string.IsNullOrWhiteSpace(Especialidades))
        {
            erros.Add(
                "O campo \"Especialidades\" deve ser preenchido."
            );
        }
        else if (Especialidades.Length > 500)
        {
            erros.Add(
                "O campo \"Especialidades\" deve conter no máximo 500 caracteres."
            );
        }

        if (DataContratacao == default)
        {
            erros.Add(
                "O campo \"Data de contratação\" deve ser preenchido."
            );
        }
        else if (DataContratacao.Date > DateTime.Today)
        {
            erros.Add(
                "A data de contratação não pode ser futura."
            );
        }

        return erros;
    }
}
