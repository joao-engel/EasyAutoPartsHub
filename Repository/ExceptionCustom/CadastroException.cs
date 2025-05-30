namespace EasyAutoPartsHub.Repository.ExceptionCustom;
internal static class CadastroException
{
    public static string ExceptionRepositoryGrupoProduto(this Exception ex)
    {
        if (ex.Message.Contains("UK_GrupoProduto"))
            return $"<b>Grupo</b> já cadastrado!" +
                $"<hr> <small><i>{ex.Message}</i></small>";

        if (ex.Message.Contains("Observacao'. Truncated"))
            return $"<b>Observação</b> excedeu o limite de 200 caracteres!" +
                $"<hr> <small><i>{ex.Message}</i></small>";

        return $"Erro não catalogado." +
                $"<hr> <small><i>{ex.Message}</i></small>";
    }

    public static string ExceptionRepositoryFornecedor(this Exception ex)
    {
        if (ex.Message.Contains("UK_Fornecedor_CNPJ"))
            return $"<b>CNPJ</b> já cadastrado! " +
                $"<hr> <small><i>{ex.Message}</i></small>";

        if (ex.Message.Contains("UK_Fornecedor_Nome"))
            return $"<b>Nome Fantasia</b> já cadastrado!" +
                $"<hr> <small><i>{ex.Message}</i></small>";

        return $"Erro não catalogado." +
                $"<hr> <small><i>{ex.Message}</i></small>";
    }

    public static string ExceptionRepositoryProduto(this Exception ex)
    {
        if (ex.Message.Contains("UK_Produto_CodigoExterno"))
            return $"<b>Código Externo</b> já cadastrado!" +
                $"<hr> <small><i>{ex.Message}</i></small>";

        if (ex.Message.Contains("UK_Produto_Descricao"))
            return $"<b>Nome do produto</b> já cadastrado!" +
                $"<hr> <small><i>{ex.Message}</i></small>";

        if (ex.Message.Contains("Descricao'. Truncated"))
            return $"<b>Descrição</b> excedeu o limite de 50 caracteres!" +
                $"<hr> <small><i>{ex.Message}</i></small>";

        return $"Erro não catalogado." +
                $"<hr> <small><i>{ex.Message}</i></small>";
    }

    public static string ExceptionRepositoryCliente(this Exception ex)
    {
        if (ex.Message.Contains("UK_Cliente"))
            return $"<b>Documento</b> já cadastrado!" +
                $"<hr> <small><i>{ex.Message}</i></small>";

        if (ex.Message.Contains("Nome'. Truncated"))
            return $"<b>Nome</b> excedeu o limite de 50 caracteres!" +
                $"<hr> <small><i>{ex.Message}</i></small>";

        return $"Erro não catalogado." +
                $"<hr> <small><i>{ex.Message}</i></small>";
    }
}
