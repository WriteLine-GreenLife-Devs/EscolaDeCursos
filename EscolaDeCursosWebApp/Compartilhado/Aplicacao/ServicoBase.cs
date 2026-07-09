using EscolaDeCursosWebApp.Compartilhado.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Compartilhado.Aplicacao;

public abstract class ServicoBase<T> where T : EntidadeBase<T>
{
    protected static Result ValidarEntidade(T entidade)
    {
        List<string> erros = entidade.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Falha(string.Empty, erros.First());
    }

    protected static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}
