using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloCategoria.Aplicacao;

public class ServicoCategoria : ServicoBase<Categoria>
{
    private readonly IRepositorioCategoria repositorioCategoria;

    public ServicoCategoria(IRepositorioCategoria repositorioCategoria)
    {
        this.repositorioCategoria = repositorioCategoria;
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    public Result CadastrarCategoria(CadastrarCategoriaDto cadastrarCategoriaDto)
    {
        var categoria = new Categoria(
            cadastrarCategoriaDto.nome,
            cadastrarCategoriaDto.descricao,
            cadastrarCategoriaDto.status
        );

        if (repositorioCategoria.SelecionarTodos()
            .Any(c => string.Equals(c.nome, categoria.nome, StringComparison.OrdinalIgnoreCase)))
        {
            return Falha(nameof(cadastrarCategoriaDto.nome), "Já existe uma categoria com esse nome.");
        }

        Result resultadoValidacao = ValidarEntidade(categoria);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCategoria.Cadastrar(categoria);
        return Result.Ok();
    }

    public Result EditarCategoria(Guid id, EditarCategoriaDto editarCategoriaDto)
    {
        var categoriaExistente = repositorioCategoria.SelecionarPorId(id);

        if (categoriaExistente == null)
            return Falha("Id", "Categoria não encontrada.");

        if (repositorioCategoria.SelecionarTodos()
            .Any(c => c.Id != id && string.Equals(c.nome, editarCategoriaDto.nome, StringComparison.OrdinalIgnoreCase)))
        {
            return Falha(nameof(editarCategoriaDto.nome), "Já existe uma categoria com esse nome.");
        }

        var categoriaAtualizada = new Categoria(
            editarCategoriaDto.nome,
            editarCategoriaDto.descricao,
            editarCategoriaDto.status
        );

        Result resultadoValidacao = ValidarEntidade(categoriaAtualizada);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        if (!repositorioCategoria.Editar(id, categoriaAtualizada))
            return Falha(string.Empty, "Falha ao atualizar a categoria.");

        return Result.Ok();
    }

    public bool ExcluirCategoria(Guid id)
    {
        return repositorioCategoria.Excluir(id);
    }
}
