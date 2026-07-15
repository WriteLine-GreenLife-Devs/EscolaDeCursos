using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAluno;

public sealed class PresencaAluno : EntidadeBase<PresencaAluno>
{
    public Guid AlunoId { get; set; }
    public Guid MatriculaId { get; set; }
    public DateTime DataAula { get; set; }
    public bool Presente { get; set; }
    public Aluno Aluno { get; set; } = null!;

    public PresencaAluno() { }

    public PresencaAluno(
        Guid alunoId,
        Guid matriculaId,
        DateTime dataAula,
        bool presente)
    {
        AlunoId = alunoId;
        MatriculaId = matriculaId;
        DataAula = dataAula.Date;
        Presente = presente;
    }

    public override void Atualizar(PresencaAluno entidadeAtualizada)
    {
        DataAula = entidadeAtualizada.DataAula.Date;
        Presente = entidadeAtualizada.Presente;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (AlunoId == Guid.Empty)
            erros.Add("O aluno da presença é obrigatório.");

        if (MatriculaId == Guid.Empty)
            erros.Add("A matrícula da presença é obrigatória.");

        if (DataAula == default)
        {
            erros.Add("A data da aula é obrigatória.");
        }
        else if (DataAula.Date > DateTime.Today)
        {
            erros.Add("A presença não pode ser registrada em uma data futura.");
        }

        return erros;
    }
}