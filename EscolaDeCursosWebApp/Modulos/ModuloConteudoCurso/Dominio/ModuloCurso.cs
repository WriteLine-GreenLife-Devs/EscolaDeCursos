using EscolaDeCursosWebApp.Compartilhado.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio;

public sealed class ModuloCurso : EntidadeBase<ModuloCurso>
{
    public Guid CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int DuracaoMinutos { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; } = true;

    public ModuloCurso() { }

    public ModuloCurso(
        Guid cursoId,
        string titulo,
        string descricao,
        int duracaoMinutos,
        int ordem)
    {
        CursoId = cursoId;
        Titulo = titulo?.Trim() ?? string.Empty;
        Descricao = descricao?.Trim() ?? string.Empty;
        DuracaoMinutos = duracaoMinutos;
        Ordem = ordem;
    }

    public override void Atualizar(ModuloCurso entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo?.Trim() ?? string.Empty;
        Descricao = entidadeAtualizada.Descricao?.Trim() ?? string.Empty;
        DuracaoMinutos = entidadeAtualizada.DuracaoMinutos;
        Ordem = entidadeAtualizada.Ordem;
        Ativo = entidadeAtualizada.Ativo;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    public void Reativar()
    {
        Ativo = true;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (CursoId == Guid.Empty)
            erros.Add("O curso do módulo é obrigatório.");

        if (string.IsNullOrWhiteSpace(Titulo))
        {
            erros.Add("O campo \"Título\" deve ser preenchido.");
        }
        else if (Titulo.Length < 3 || Titulo.Length > 100)
        {
            erros.Add(
                "O campo \"Título\" deve conter entre 3 e 100 caracteres."
            );
        }

        if (string.IsNullOrWhiteSpace(Descricao))
        {
            erros.Add("O campo \"Descrição\" deve ser preenchido.");
        }
        else if (Descricao.Length < 3 || Descricao.Length > 1000)
        {
            erros.Add(
                "O campo \"Descrição\" deve conter entre 3 e 1000 caracteres."
            );
        }

        if (DuracaoMinutos <= 0)
            erros.Add("A duração do módulo deve ser maior que zero.");

        if (Ordem <= 0)
            erros.Add("A ordem do módulo deve ser maior que zero.");

        return erros;
    }
}