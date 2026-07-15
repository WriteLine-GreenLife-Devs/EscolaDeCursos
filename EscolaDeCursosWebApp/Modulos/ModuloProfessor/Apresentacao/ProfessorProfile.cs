using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloProfessor;

namespace EscolaDeCursosWebApp.Modulos.ModuloProfessor.Apresentacao;

public sealed class ProfessorProfile : Profile
{
    public ProfessorProfile()
    {
        CreateMap<CadastrarProfessorPerfilViewModel, CadastrarProfessorPerfilDto>();
        CreateMap<EditarProfessorPerfilViewModel, EditarProfessorPerfilDto>();
        CreateMap<ListarProfessoresDto, ListarProfessoresViewModel>();
        CreateMap<DetalhesProfessorDto, DetalhesProfessorViewModel>();
        CreateMap<DetalhesProfessorDto, EditarProfessorPerfilViewModel>();
    }
}