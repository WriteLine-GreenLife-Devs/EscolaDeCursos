using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloUsuario;

public sealed class Usuario : EntidadeBase<Usuario>
{
    public string nome { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string senha { get; set; } = string.Empty;
    public string telefone { get; set; } = string.Empty;
    public TipoUsuario tipoUsuario { get; set; } = TipoUsuario.Aluno;
    public bool ativo { get; set; } = true;

    public Usuario() { }
    public Usuario(string nome, string email, string senha, string telefone, TipoUsuario tipoUsuario)
    {
        this.nome = nome;
        this.email = email;
        this.senha = senha;
        this.telefone = telefone;
        this.tipoUsuario = tipoUsuario;
    }

    public override void Atualizar(Usuario entidadeAtualizada)
    {
        nome = entidadeAtualizada.nome;
        email = entidadeAtualizada.email;
        senha = entidadeAtualizada.senha;
        telefone = entidadeAtualizada.telefone;
        tipoUsuario = entidadeAtualizada.tipoUsuario;
        ativo = entidadeAtualizada.ativo;
    }

    public string VerificarTelefone(string Telefone)
    {
        string apenasNumeros = System.Text.RegularExpressions.Regex.Replace(Telefone ?? "", @"[^\d]", "");

        int tamanho = apenasNumeros.Length;

        if (tamanho == 10)
        {
            telefone = long.Parse(apenasNumeros).ToString(@"(00) 0000-0000");
            return long.Parse(apenasNumeros).ToString(@"(00) 0000-0000");
        }
        else if (tamanho == 11)
        {
            telefone = long.Parse(apenasNumeros).ToString(@"(00) 00000-0000");
            return long.Parse(apenasNumeros).ToString(@"(00) 00000-0000");
        }
        else
        {
            return "";
        }
    }

    public static bool VerificarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(email.Trim(),
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(nome))
            erros.Add("O campo \"Nome\" deve ser preenchido.");

        else if (nome.Length < 2 || nome.Length > 100)
            erros.Add("O campo \"Nome\" deve conter entre 2 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(email))
            erros.Add("O campo \"Email\" deve ser preenchido.");

        else if (!VerificarEmail(email))
            erros.Add("O campo \"Email\" é inválido.");

        if (string.IsNullOrWhiteSpace(senha))
            erros.Add("O campo \"Senha\" deve ser preenchido.");

        if (string.IsNullOrWhiteSpace(telefone))
            erros.Add("O campo \"Telefone\" deve ser preenchido.");

        else if (VerificarTelefone(telefone) == "")
            erros.Add("O campo \"Telefone\" é inválido.");

        return erros;
    }
}
