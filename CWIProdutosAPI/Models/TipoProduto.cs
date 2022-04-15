using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWIProdutosAPI.Models
{
    public class TipoProduto 
    {
        public TipoProduto()
        {
        }

        public TipoProduto(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public int Id { get; set; }
        public string Descricao { get; set; }

        public TipoProdutoTO ConverterEmTO() 
        {
            return new TipoProdutoTO(this.Id, this.Descricao);
        }
    }
}
