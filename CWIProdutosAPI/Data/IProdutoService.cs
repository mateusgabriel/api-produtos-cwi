using CWIProdutosAPI.Models;
using System.Collections.Generic;

namespace CWIProdutosAPI.Data
{
    public interface IProdutoService
    {
        List<ProdutoTO> Listar();

        RetornoServico Consultar(int produtoId);

        RetornoServico Salvar(ProdutoTO produto);

        string Excluir(int produtoId);

        bool ProdutoExiste(int produtoId);

    }
}
