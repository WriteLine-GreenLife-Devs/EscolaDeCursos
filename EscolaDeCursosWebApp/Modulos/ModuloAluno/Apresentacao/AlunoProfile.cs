using AutoMapper;
using EscolaDeCursosWebApp.Modulos.ModuloAluno.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Apresentacao;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Apresentacao;

public sealed class AlunoProfile : Profile
{
    public AlunoProfile()
    {
        CreateMap<CadastrarUsuarioViewModel, CadastrarAlunoDto>();
        CreateMap<DetalhesAlunoDto, DetalhesAlunoViewModel>();
        CreateMap<MatriculaPainelAlunoDto, MatriculaPainelAlunoViewModel>();
        CreateMap<NotaAlunoDto, NotaAlunoViewModel>();
        CreateMap<PresencaAlunoDto, PresencaAlunoViewModel>();
    }
}