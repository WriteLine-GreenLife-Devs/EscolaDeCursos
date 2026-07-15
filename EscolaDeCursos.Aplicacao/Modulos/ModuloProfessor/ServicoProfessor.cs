using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloProfessor;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloProfessor;

public sealed class ServicoProfessor
    : ServicoBase<Professor>
{
    private readonly IRepositorioProfessor repositorioProfessor;
    private readonly IRepositorioUsuario repositorioUsuario;

    public ServicoProfessor(
        IRepositorioProfessor repositorioProfessor,
        IRepositorioUsuario repositorioUsuario)
    {
        this.repositorioProfessor = repositorioProfessor;
        this.repositorioUsuario = repositorioUsuario;
    }

    public Result Cadastrar(CadastrarProfessorDto dto)
    {
        var usuario = new Usuario(
            dto.Nome,
            dto.Email,
            HashSenha.GerarHash(dto.Senha),
            dto.Telefone,
            TipoUsuario.Professor
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

        var professor = new Professor(
            usuario.Id,
            dto.Bio,
            dto.Especialidades,
            dto.DataContratacao
        );

        Result resultadoValidacao = ValidarEntidade(professor);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioProfessor.CadastrarProfessor(usuario, professor);

        return Result.Ok();
    }

    public Result Cadastrar(CadastrarProfessorPerfilDto dto)
    {
        if (dto.UsuarioId == Guid.Empty)
        {
            return Falha(
                nameof(dto.UsuarioId),
                "O usuário do professor é obrigatório."
            );
        }

        Usuario? usuario =
            repositorioUsuario.SelecionarPorId(dto.UsuarioId);

        if (usuario == null)
        {
            return Falha(
                nameof(dto.UsuarioId),
                "Usuário não encontrado."
            );
        }

        if (usuario.tipoUsuario != TipoUsuario.Professor)
        {
            return Falha(
                nameof(dto.UsuarioId),
                "O usuário selecionado não é um professor."
            );
        }

        Professor? perfilExistente =
            repositorioProfessor.SelecionarPorId(dto.UsuarioId);

        if (perfilExistente != null)
        {
            return Falha(
                nameof(dto.UsuarioId),
                "Este usuário já possui um perfil de professor."
            );
        }

        var professor = new Professor(
            dto.UsuarioId,
            dto.Bio,
            dto.Especialidades,
            dto.DataContratacao
        );

        Result resultadoValidacao =
            ValidarEntidade(professor);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioProfessor.Cadastrar(professor);

        return Result.Ok();
    }

    public Result Editar(EditarProfessorPerfilDto dto)
    {
        Professor? perfilExistente =
            repositorioProfessor.SelecionarPorId(dto.Id);

        if (perfilExistente == null)
        {
            return Falha(
                nameof(dto.Id),
                "Perfil de professor não encontrado."
            );
        }

        var perfilAtualizado = new Professor(
            dto.Id,
            dto.Bio,
            dto.Especialidades,
            dto.DataContratacao
        );

        Result resultadoValidacao =
            ValidarEntidade(perfilAtualizado);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        bool conseguiuEditar =
            repositorioProfessor.Editar(
                dto.Id,
                perfilAtualizado
            );

        if (!conseguiuEditar)
        {
            return Falha(
                string.Empty,
                "Não foi possível atualizar o perfil do professor."
            );
        }

        return Result.Ok();
    }

    public List<ListarProfessoresDto> SelecionarTodos()
    {
        List<Professor> perfis =
            repositorioProfessor.SelecionarTodos();

        var professores = new List<ListarProfessoresDto>();

        foreach (Professor perfil in perfis)
        {
            Usuario? usuario =
                repositorioUsuario.SelecionarPorId(perfil.Id);

            if (usuario == null)
                continue;

            professores.Add(
                MapearParaListagem(perfil, usuario)
            );
        }

        return professores;
    }

    public DetalhesProfessorDto? SelecionarPorId(Guid id)
    {
        Professor? perfil =
            repositorioProfessor.SelecionarPorId(id);

        if (perfil == null)
            return null;

        Usuario? usuario =
            repositorioUsuario.SelecionarPorId(id);

        if (usuario == null)
            return null;

        return MapearParaDetalhes(perfil, usuario);
    }

    public DetalhesProfessorDto? SelecionarDadosParaPerfil(Guid usuarioId)
    {
        Usuario? usuario = repositorioUsuario.SelecionarPorId(usuarioId);

        if (usuario == null || usuario.tipoUsuario != TipoUsuario.Professor)
            return null;

        Professor? professor = repositorioProfessor.SelecionarPorId(usuarioId);

        return new DetalhesProfessorDto(
            usuario.Id,
            usuario.nome,
            usuario.email,
            usuario.telefone,
            professor?.Bio ?? string.Empty,
            professor?.Especialidades ?? string.Empty,
            professor?.DataContratacao ?? DateTime.Today
        );
    }

    private static ListarProfessoresDto MapearParaListagem(
        Professor perfil,
        Usuario usuario)
    {
        return new ListarProfessoresDto(
            perfil.Id,
            usuario.nome,
            usuario.email,
            perfil.Especialidades,
            perfil.DataContratacao
        );
    }

    private static DetalhesProfessorDto MapearParaDetalhes(
        Professor perfil,
        Usuario usuario)
    {
        return new DetalhesProfessorDto(
            perfil.Id,
            usuario.nome,
            usuario.email,
            usuario.telefone,
            perfil.Bio,
            perfil.Especialidades,
            perfil.DataContratacao
        );
    }

    private static Result MapearFalhaUsuario(string erro)
    {
        string campo = erro.Contains("Nome") ? nameof(CadastrarProfessorDto.Nome)
            : erro.Contains("Telefone") ? nameof(CadastrarProfessorDto.Telefone)
            : erro.Contains("Email") ? nameof(CadastrarProfessorDto.Email)
            : erro.Contains("Senha") ? nameof(CadastrarProfessorDto.Senha)
            : string.Empty;

        return Falha(campo, erro);
    }
}