using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWIProdutosAPI.Models
{
    public class TipoProdutoTO
    {
        public TipoProdutoTO()
        {
        }

        public TipoProdutoTO(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public int Id { get; set; }
        public string Descricao { get; set; }

        public TipoProduto ConverterEmEntidade() 
        {
            return new TipoProduto(this.Id, this.Descricao);
        }
    }
}
