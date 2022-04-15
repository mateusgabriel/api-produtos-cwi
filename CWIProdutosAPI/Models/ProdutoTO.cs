using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWIProdutosAPI.Models
{
    public class ProdutoTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal? Valor { get; set; }
        public DateTime Cadastro { get; set; }
        public int Quantidade { get; set; }
        public TipoProdutoTO Tipo { get; set; }

        public Produto ConverterEmEntidade() 
        {
            return new Produto()
            {
                Id = this.Id,
                Descricao = this.Descricao,
                Quantidade = this.Quantidade,
                Cadastro = this.Cadastro,
                Valor = this.Valor,
                TipoProduto = this.Tipo.ConverterEmEntidade()
            };
        }
    }
}
