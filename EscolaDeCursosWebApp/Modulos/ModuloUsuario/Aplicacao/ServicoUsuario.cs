using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;

public class ServicoUsuario : ServicoBase<Usuario>
{
    private readonly IRepositorioUsuario repositorioUsuario;

    public ServicoUsuario(IRepositorioUsuario repositorioUsuario)
    {
        this.repositorioUsuario = repositorioUsuario;
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
    private bool VerificarEmailAndTelefoneExistente(string Email, string Telefone)
    {
        return repositorioUsuario.SelecionarTodos().Any(c => c.email == Email && c.telefone == Telefone);
    }
    private bool VerificarEmailAndTelefoneExistenteEditar(string Email, string Telefone, Guid id)
    {
        return repositorioUsuario.SelecionarTodos().Any(c => c.email == Email && c.telefone == Telefone && c.Id != id);
    }

    private static Result ValidarEntidade(Usuario usuario)
    {
        List<string> erros = usuario.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        string erro = erros.First();
        string campo = erro.Contains("Nome") ? nameof(Usuario.nome)
            : erro.Contains("Telefone") ? nameof(Usuario.telefone)
            : erro.Contains("Email") ? nameof(Usuario.email)
            : string.Empty;

        return Result.Fail(new Error(erro).WithMetadata("Campo", campo));
    }

    public Result CadastrarUsuario(CadastrarUsuarioDto cadastrarUsuarioDto)
    {
        // Verificações separadas para evitar violação de índices únicos no banco
        var todos = repositorioUsuario.SelecionarTodos();

        if (todos.Any(c => string.Equals(c.email, cadastrarUsuarioDto.email, StringComparison.OrdinalIgnoreCase)))
            return Falha("Email", "Já existe um usuário com esse email.");

        if (todos.Any(c => c.telefone == cadastrarUsuarioDto.telefone))
            return Falha("Telefone", "Já existe um usuário com esse telefone.");

        Usuario usuario = new Usuario(
            cadastrarUsuarioDto.nome,
            cadastrarUsuarioDto.email,
            cadastrarUsuarioDto.senha,
            cadastrarUsuarioDto.telefone,
            cadastrarUsuarioDto.tipoUsuario
        );

        Result resultadoValidacao = ValidarEntidade(usuario);

        if(resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioUsuario.Cadastrar(usuario);

        return Result.Ok();
    }
}
