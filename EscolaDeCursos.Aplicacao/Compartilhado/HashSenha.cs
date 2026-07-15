namespace EscolaDeCursos.Aplicacao.Compartilhado;

public static class HashSenha
{
    public static string GerarHash(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 12);
    }

    public static bool Verificar(string senha, string hash)
    {
        if (string.IsNullOrEmpty(hash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }
        catch
        {
            return false;
        }
    }
}