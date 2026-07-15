using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;
using EscolaDeCursosWebApp.Modulos.ModuloUsuario.Apresentacao;

namespace EscolaDeCursosWebApp.Modulos.ModuloAluno.Apresentacao;

public sealed class AlunoProfile : Profile
{
    public AlunoProfile()
    {
        CreateMap<CadastrarUsuarioViewModel, CadastrarAlunoDto>();
        CreateMap<DetalhesAlunoDto, DetalhesAlunoViewModel>();
        CreateMap<TurmaResumoAlunoDto, TurmaResumoAlunoViewModel>();
        CreateMap<CursoResumoAlunoDto, CursoResumoAlunoViewModel>();
        CreateMap<ProfessorResumoAlunoDto, ProfessorResumoAlunoViewModel>();
        CreateMap<MatriculaPainelAlunoDto, MatriculaPainelAlunoViewModel>();
        CreateMap<TurmaCatalogoAlunoDto, TurmaCatalogoAlunoViewModel>();
        CreateMap<CursoCatalogoAlunoDto, CursoCatalogoAlunoViewModel>();
        CreateMap<NotaAlunoDto, NotaAlunoViewModel>();
        CreateMap<PresencaAlunoDto, PresencaAlunoViewModel>();
    }
}