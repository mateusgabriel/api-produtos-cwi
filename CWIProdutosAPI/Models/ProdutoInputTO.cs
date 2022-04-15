using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWIProdutosAPI.Models
{
    public class ProdutoInputTO
    {
        public string Descricao { get; set; }
        public decimal? Valor { get; set; }
        public DateTime Cadastro { get; set; }
        public int Quantidade { get; set; }
        public string Tipo { get; set; }

        public ProdutoTO ConverterEmTO()
        {
            return new ProdutoTO()
            {
                Descricao = this.Descricao,
                Quantidade = this.Quantidade,
                Cadastro = this.Cadastro,
                Valor = this.Valor,
                Tipo = new TipoProdutoTO(0, this.Tipo.Trim())
            };
        }
    }
}
