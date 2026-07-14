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
            HashSenha.GerarHash(cadastrarUsuarioDto.senha),
            cadastrarUsuarioDto.telefone,
            cadastrarUsuarioDto.tipoUsuario
        );

        Result resultadoValidacao = ValidarEntidade(usuario);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioUsuario.Cadastrar(usuario);

        return Result.Ok();
    }

    public Usuario? AutenticarUsuario(string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            return null;

        var encontrados = repositorioUsuario.Filtrar(u =>
            u.ativo &&
            string.Equals(u.email, email, StringComparison.OrdinalIgnoreCase));

        var usuario = encontrados.FirstOrDefault();

        if (usuario == null)
            return null;

        if (!HashSenha.Verificar(senha, usuario.senha))
            return null;

        return usuario;
    }

    public bool DesativarUsuario(Guid usuarioId, TipoUsuario tipoUsuarioEsperado)
    {
        Usuario? usuario =
            repositorioUsuario.SelecionarPorId(usuarioId);

        if (usuario == null ||
            !usuario.ativo ||
            usuario.tipoUsuario != tipoUsuarioEsperado)
        {
            return false;
        }

        var usuarioDesativado = new Usuario(
            usuario.nome,
            usuario.email,
            usuario.senha,
            usuario.telefone,
            usuario.tipoUsuario
        )
        {
            ativo = false
        };

        return repositorioUsuario.Editar(usuarioId, usuarioDesativado);
    }

    public bool VerificarUsuarioAtivo(Guid usuarioId)
    {
        Usuario? usuario =
            repositorioUsuario.SelecionarPorId(usuarioId);

        return usuario != null && usuario.ativo;
    }
}
