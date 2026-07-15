using EscolaDeCursos.Dominio.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAluno;

public sealed class Aluno : EntidadeBase<Aluno>
{
    public Usuario Usuario { get; set; } = null!;
    public List<NotaAluno> Notas { get; set; } = [];
    public List<PresencaAluno> Presencas { get; set; } = [];

    public Aluno() { }

    public Aluno(Guid usuarioId)
    {
        Id = usuarioId;
    }

    public override void Atualizar(Aluno entidadeAtualizada) { }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (Id == Guid.Empty)
            erros.Add("O aluno deve estar vinculado a um usuário.");

        return erros;
    }
}