using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;

public sealed class ServicoAluno : ServicoBase<Aluno>
{
    private readonly IRepositorioAluno repositorioAluno;
    private readonly IRepositorioUsuario repositorioUsuario;

    public ServicoAluno(
        IRepositorioAluno repositorioAluno,
        IRepositorioUsuario repositorioUsuario)
    {
        this.repositorioAluno = repositorioAluno;
        this.repositorioUsuario = repositorioUsuario;
    }

    public Result Cadastrar(CadastrarAlunoDto dto)
    {
        var usuario = new Usuario(
            dto.Nome,
            dto.Email,
            dto.Senha,
            dto.Telefone,
            TipoUsuario.Aluno
        );

        List<string> errosUsuario = usuario.Validar();

        if (errosUsuario.Count > 0)
            return MapearFalhaUsuario(errosUsuario.First());

        List<Usuario> usuarios = repositorioUsuario.SelecionarTodos();

        if (usuarios.Any(u => string.Equals(
            u.email,
            usuario.email,
            StringComparison.OrdinalIgnoreCase)))
        {
            return Falha(nameof(dto.Email), "Já existe um usuário com esse email.");
        }

        if (usuarios.Any(u => u.telefone == usuario.telefone))
            return Falha(nameof(dto.Telefone), "Já existe um usuário com esse telefone.");

        var aluno = new Aluno(usuario.Id);
        Result resultadoValidacao = ValidarEntidade(aluno);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioAluno.CadastrarAluno(usuario, aluno);

        return Result.Ok();
    }

    public List<ListarAlunosDto> SelecionarTodos()
    {
        var alunos = new List<ListarAlunosDto>();

        foreach (Aluno aluno in repositorioAluno.SelecionarTodos())
        {
            Usuario? usuario = repositorioUsuario.SelecionarPorId(aluno.Id);

            if (usuario == null)
                continue;

            alunos.Add(MapearParaListagem(usuario));
        }

        return alunos;
    }

    public DetalhesAlunoDto? SelecionarPorId(Guid alunoId)
    {
        Aluno? aluno = repositorioAluno.SelecionarPorId(alunoId);

        if (aluno == null)
            return null;

        Usuario? usuario = repositorioUsuario.SelecionarPorId(alunoId);

        return usuario == null
            ? null
            : MapearParaDetalhes(usuario);
    }

    private static ListarAlunosDto MapearParaListagem(Usuario usuario)
    {
        return new ListarAlunosDto(
            usuario.Id,
            usuario.nome,
            usuario.email,
            usuario.telefone,
            usuario.ativo
        );
    }

    private static DetalhesAlunoDto MapearParaDetalhes(Usuario usuario)
    {
        return new DetalhesAlunoDto(
            usuario.Id,
            usuario.nome,
            usuario.email,
            usuario.telefone,
            usuario.ativo
        );
    }

    private static Result MapearFalhaUsuario(string erro)
    {
        string campo = erro.Contains("Nome") ? nameof(CadastrarAlunoDto.Nome)
            : erro.Contains("Telefone") ? nameof(CadastrarAlunoDto.Telefone)
            : erro.Contains("Email") ? nameof(CadastrarAlunoDto.Email)
            : erro.Contains("Senha") ? nameof(CadastrarAlunoDto.Senha)
            : string.Empty;

        return Falha(campo, erro);
    }
}