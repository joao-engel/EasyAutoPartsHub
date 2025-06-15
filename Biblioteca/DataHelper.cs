public static class DataHelper
{
    private static readonly string[] MesesAbreviados =
            { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun",
              "Jul", "Ago", "Set", "Out", "Nov", "Dez" };

    public static string GetMesAbreviado(int mes)
    {
        if (mes < 1 || mes > 12)
            throw new ArgumentOutOfRangeException(nameof(mes), "O mês deve estar entre 1 e 12.");

        return MesesAbreviados[mes - 1];
    }

    public static string GetMesAnoAbreviado(int mes, int ano)
    {
        return $"{GetMesAbreviado(mes)}/{ano}";
    }
}