using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloCurso;

public sealed class Curso : EntidadeBase<Curso>
{

    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
    public int cargaHoraria { get; set; } = 0;
    public NivelDificuldade nivelDificuldade { get; set; } = NivelDificuldade.Iniciante;
    public StatusCurso status { get; set; } = StatusCurso.Ativo;
    public decimal valor { get; set; } = 0;
    public DateTime dataCriacao { get; set; } = DateTime.Now;
    public Guid categoriaId { get; set; }
    public override void Atualizar(Curso entidadeAtualizada)
    {
        this.nome = entidadeAtualizada.nome;
        this.descricao = entidadeAtualizada.descricao;
        this.cargaHoraria = entidadeAtualizada.cargaHoraria;
        this.nivelDificuldade = entidadeAtualizada.nivelDificuldade;
        this.status = entidadeAtualizada.status;
        this.valor = entidadeAtualizada.valor;
        this.categoriaId = entidadeAtualizada.categoriaId;
    }

    public override List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(nome))
            erros.Add("O nome do curso é obrigatório.");

        else if (nome.Length < 3 || nome.Length > 100)
            erros.Add("O nome do curso deve ter entre 3 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(descricao))
            erros.Add("A descrição do curso é obrigatória.");

        else if (descricao.Length < 3 || descricao.Length > 200)
            erros.Add("A descrição do curso deve ter entre 3 e 200 caracteres.");

        if (cargaHoraria <= 0)
            erros.Add("A carga horária do curso deve ser maior que zero.");

        if (valor < 0)
            erros.Add("O valor do curso não pode ser negativo.");

        return erros;
    }
}
