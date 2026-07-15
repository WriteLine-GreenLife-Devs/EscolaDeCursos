using AutoMapper;
using EscolaDeCursos.Aplicacao.Modulos.ModuloConteudoCurso;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Apresentacao;

public sealed class ModuloConteudoCursoProfile : Profile
{
    public ModuloConteudoCursoProfile()
    {
        CreateMap<CadastrarModuloCursoViewModel, CadastrarModuloCursoDto>();
        CreateMap<EditarModuloCursoViewModel, EditarModuloCursoDto>();
        CreateMap<ModuloCursoDto, ModuloCursoViewModel>();
        CreateMap<AtualizarConclusaoModuloAlunoViewModel,
            AtualizarConclusaoModuloAlunoDto>();
        CreateMap<ModuloProgressoAlunoDto,
            ModuloProgressoAlunoViewModel>();
        CreateMap<ResumoProgressoModuloAlunoDto,
            ResumoProgressoModuloAlunoViewModel>();
    }
}