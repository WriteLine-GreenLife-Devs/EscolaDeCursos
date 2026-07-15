using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloCategoria;

public sealed class Categoria : EntidadeBase<Categoria>
{
    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
    public DateTime dataCriacao { get; set; } = DateTime.Now;
    public StatusCategoria status { get; set; } = StatusCategoria.Ativo;

    public Categoria()
    {
    }
    public Categoria(string nome, string descricao, StatusCategoria status)
    {
        this.nome = nome;
        this.descricao = descricao;
        this.status = status;
    }
    public override void Atualizar(Categoria entidadeAtualizada)
    {
        this.nome = entidadeAtualizada.nome;
        this.descricao = entidadeAtualizada.descricao;
        this.status = entidadeAtualizada.status;
    }

    public override List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(nome))
            erros.Add("O nome da categoria é obrigatório.");

        else if (nome.Length < 3 || nome.Length > 100)
            erros.Add("O nome da categoria deve ter entre 3 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(descricao))
            erros.Add("A descrição da categoria é obrigatória.");

        else if (descricao.Length < 3 || descricao.Length > 200)
            erros.Add("A descrição da categoria deve ter entre 3 e 200 caracteres.");

        return erros;
    }
}
