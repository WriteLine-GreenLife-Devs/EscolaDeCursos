namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Aplicacao;

public record CadastrarProfessorDto(
    string Nome,
    string Email,
    string Senha,
    string Telefone,
    string Bio,
    string Especialidades,
    DateTime DataContratacao
);

public record CadastrarProfessorPerfilDto(
    Guid UsuarioId,
    string Bio,
    string Especialidades,
    DateTime DataContratacao
);

public record EditarProfessorPerfilDto(
    Guid Id,
    string Bio,
    string Especialidades,
    DateTime DataContratacao
);

public record ListarProfessoresDto(
    Guid Id,
    string Nome,
    string Email,
    string Especialidades,
    DateTime DataContratacao
);

public record DetalhesProfessorDto(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string Bio,
    string Especialidades,
    DateTime DataContratacao
);