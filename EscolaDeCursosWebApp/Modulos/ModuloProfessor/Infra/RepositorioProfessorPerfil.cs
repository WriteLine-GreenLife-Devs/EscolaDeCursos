using EscolaDeCursosWebApp.Compartilhado.Infra.Orm;
using EscolaDeCursosWebApp.Modulos.ModuloProfessor.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Infra;

public sealed class RepositorioProfessorPerfil(
    EscolaDeCursosDbContext dbContext)
    : RepositorioBase<ProfessorPerfil>(dbContext),
      IRepositorioProfessorPerfil { }