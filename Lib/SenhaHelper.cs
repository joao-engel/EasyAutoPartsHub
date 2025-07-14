using System.Security.Cryptography;

public static class SenhaHelper
{
    public static void GerarHashSenha(string senha, out string hash, out string salt)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
        salt = Convert.ToBase64String(saltBytes);

        var pbkdf2 = new Rfc2898DeriveBytes(senha, saltBytes, 10000, HashAlgorithmName.SHA256);
        hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
    }

    public static bool VerificarSenha(string senha, string hashSalvo, string saltSalvo)
    {
        byte[] saltBytes = Convert.FromBase64String(saltSalvo);
        var pbkdf2 = new Rfc2898DeriveBytes(senha, saltBytes, 10000, HashAlgorithmName.SHA256);
        var hashCalculado = Convert.ToBase64String(pbkdf2.GetBytes(32));

        return hashSalvo == hashCalculado;
    }
}