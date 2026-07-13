using System;
using System.Linq;
using EscolaDeCursosWebApp.Compartilhado.Aplicacao;
using EscolaDeCursosWebApp.Modulos.ModuloCategoria.Dominio;
using EscolaDeCursosWebApp.Modulos.ModuloCurso.Dominio;
using FluentResults;

namespace EscolaDeCursosWebApp.Modulos.ModuloCurso.Aplicacao;

public sealed class ServicoCurso : ServicoBase<Curso>
{
    private readonly IRepositorioCurso repositorioCurso;
    private readonly IRepositorioCategoria repositorioCategoria;

    public ServicoCurso(
        IRepositorioCurso repositorioCurso,
        IRepositorioCategoria repositorioCategoria
    )
    {
        this.repositorioCurso = repositorioCurso;
        this.repositorioCategoria = repositorioCategoria;
    }

    public Result CadastrarCurso(CadastrarCursoDto cadastrarCursoDto)
    {
        if (repositorioCategoria.SelecionarPorId(cadastrarCursoDto.categoriaId) == null)
            return Falha(nameof(cadastrarCursoDto.categoriaId), "Categoria não encontrada.");

        if (repositorioCurso.SelecionarTodos()
            .Any(c => string.Equals(c.nome, cadastrarCursoDto.nome, StringComparison.OrdinalIgnoreCase)))
        {
            return Falha(nameof(cadastrarCursoDto.nome), "Já existe um curso com esse nome.");
        }

        var curso = new Curso
        {
            nome = cadastrarCursoDto.nome,
            descricao = cadastrarCursoDto.descricao,
            cargaHoraria = cadastrarCursoDto.cargaHoraria,
            nivelDificuldade = cadastrarCursoDto.nivelDificuldade,
            status = cadastrarCursoDto.status,
            valor = cadastrarCursoDto.valor,
            categoriaId = cadastrarCursoDto.categoriaId
        };

        Result resultadoValidacao = ValidarEntidade(curso);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCurso.Cadastrar(curso);
        return Result.Ok();
    }

    public Result EditarCurso(Guid id, EditarCursoDto editarCursoDto)
    {
        var cursoExistente = repositorioCurso.SelecionarPorId(id);

        if (cursoExistente == null)
            return Falha("Id", "Curso não encontrado.");

        if (repositorioCategoria.SelecionarPorId(editarCursoDto.categoriaId) == null)
            return Falha(nameof(editarCursoDto.categoriaId), "Categoria não encontrada.");

        if (repositorioCurso.SelecionarTodos()
            .Any(c => c.Id != id && string.Equals(c.nome, editarCursoDto.nome, StringComparison.OrdinalIgnoreCase)))
        {
            return Falha(nameof(editarCursoDto.nome), "Já existe um curso com esse nome.");
        }

        var cursoAtualizado = new Curso
        {
            nome = editarCursoDto.nome,
            descricao = editarCursoDto.descricao,
            cargaHoraria = editarCursoDto.cargaHoraria,
            nivelDificuldade = editarCursoDto.nivelDificuldade,
            status = editarCursoDto.status,
            valor = editarCursoDto.valor,
            categoriaId = editarCursoDto.categoriaId
        };

        Result resultadoValidacao = ValidarEntidade(cursoAtualizado);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        if (!repositorioCurso.Editar(id, cursoAtualizado))
            return Falha(string.Empty, "Falha ao atualizar o curso.");

        return Result.Ok();
    }

    public bool ExcluirCurso(Guid id)
    {
        return repositorioCurso.Excluir(id);
    }
}
