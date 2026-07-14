using EscolaDeCursosWebApp.Compartilhado.Dominio;

namespace EscolaDeCursosWebApp.Modulos.ModuloConteudoCurso.Dominio;

public sealed class ProgressoModuloAluno
    : EntidadeBase<ProgressoModuloAluno>
{
    public Guid MatriculaId { get; set; }
    public Guid ModuloCursoId { get; set; }
    public bool Concluido { get; set; }
    public DateTime? DataConclusao { get; set; }

    public ProgressoModuloAluno() { }

    public ProgressoModuloAluno(
        Guid matriculaId,
        Guid moduloCursoId)
    {
        MatriculaId = matriculaId;
        ModuloCursoId = moduloCursoId;
    }

    public override void Atualizar(
        ProgressoModuloAluno entidadeAtualizada)
    {
        Concluido = entidadeAtualizada.Concluido;
        DataConclusao = entidadeAtualizada.DataConclusao;
    }

    public void MarcarComoConcluido()
    {
        Concluido = true;
        DataConclusao = DateTime.Now;
    }

    public void MarcarComoPendente()
    {
        Concluido = false;
        DataConclusao = null;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (MatriculaId == Guid.Empty)
            erros.Add("A matrícula do progresso é obrigatória.");

        if (ModuloCursoId == Guid.Empty)
            erros.Add("O módulo do progresso é obrigatório.");

        if (Concluido && !DataConclusao.HasValue)
        {
            erros.Add(
                "A data de conclusão deve ser informada para um módulo concluído."
            );
        }

        if (!Concluido && DataConclusao.HasValue)
        {
            erros.Add(
                "Um módulo pendente não pode possuir data de conclusão."
            );
        }

        if (DataConclusao.HasValue &&
            DataConclusao.Value > DateTime.Now)
        {
            erros.Add("A data de conclusão não pode ser futura.");
        }

        return erros;
    }
}