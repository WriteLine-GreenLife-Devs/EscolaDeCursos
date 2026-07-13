using AutoMapper;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Aplicacao;

namespace EscolaDeCursosWebApp.Modulos.ModuloUsuario.Apresentacao;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CadastrarUsuarioViewModel, CadastrarUsuarioDto>()
            .ForMember(dest => dest.nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.senha, opt => opt.MapFrom(src => src.Senha))
            .ForMember(dest => dest.telefone, opt => opt.MapFrom(src => src.Telefone))
            .ForMember(dest => dest.tipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario));
    }
}
