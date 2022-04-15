using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWIProdutosAPI.Models
{
    public class RetornoServico
    {
        public RetornoServico()
        {
        }

        public RetornoServico(ProdutoTO produto, string mensagem)
        {
            Produto = produto;
            Mensagem = mensagem;
        }

        public ProdutoTO Produto { get; set; }
        public string Mensagem { get; set; }
        public int Codigo { get; set; }
    }
}
