using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWIProdutosAPI.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal? Valor { get; set; }
        public DateTime Cadastro { get; set; }
        public int Quantidade { get; set; }
        public int TipoProdutoId { get; set; }
        public TipoProduto TipoProduto { get; set; }

        public ProdutoTO ConverterEmTO() 
        {
            return new ProdutoTO()
            {
                Id = this.Id,
                Descricao = this.Descricao,
                Cadastro = this.Cadastro,
                Quantidade = this.Quantidade,
                Tipo = this.TipoProduto.ConverterEmTO(),
                Valor = this.Valor
            };
        }
    }
}
