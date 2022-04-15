using CWIProdutosAPI.Models;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CWIProdutosAPI.Data
{
    public class ProdutoService : IProdutoService
    {
        private readonly CWIProdutosContext context;
        private static NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ProdutoService(CWIProdutosContext context)
        {
            this.context = context;
        }

        public List<ProdutoTO> Listar() 
        {
            try
            {
                var produtos = context.Produtos.ToList();
                if (!produtos.Any()) return null;

                produtos.ForEach(p => p.TipoProduto = context.TiposProdutos.FirstOrDefault(f => f.Id == p.TipoProdutoId));
                return produtos.Select(p => p.ConverterEmTO()).ToList();
            }
            catch (Exception ex) 
            {
                Logar(ex, "Falha ao listar os produtos.");
                throw ex;
            }
        }

        public RetornoServico Consultar(int produtoId) 
        {
            var retorno = new RetornoServico();

            try
            {
                var produto = context.Produtos.Where(p => p.Id == produtoId).FirstOrDefault();
                if (produto == null) 
                {
                    retorno.Mensagem = "Produto inválido ou inexistente.";
                    return retorno;
                }

                produto.TipoProduto = context.TiposProdutos.FirstOrDefault(f => f.Id == produto.TipoProdutoId);
                retorno.Produto = produto.ConverterEmTO();
                return retorno;
            }
            catch (Exception ex) 
            {
                var mensagem = $"Falha ao consultar o produto {produtoId}.";
                Logar(ex, mensagem);

                retorno.Mensagem = mensagem;
                retorno.Codigo = 1;
                return retorno;
            }
        }

        public RetornoServico Salvar(ProdutoTO produtoTO) 
        {
            var retorno = new RetornoServico();

            var mensagem = PodeSalvar(produtoTO);

            if (!string.IsNullOrEmpty(mensagem)) 
            {
                retorno.Mensagem = mensagem;
                return retorno;
            }

            if (!ProdutoExiste(produtoTO.Id))
            {
                Inserir(produtoTO, out mensagem);
            }
            else 
            {
                Atualizar(produtoTO, out mensagem);
            }

            retorno.Produto = Consultar(produtoTO.Id).Produto;
            retorno.Mensagem = mensagem;
            return retorno;
        }

        public string PodeSalvar(ProdutoTO produtoTO) 
        {
            if (string.IsNullOrEmpty(produtoTO.Descricao))
            {
                return "O produto deve conter uma descrição.";
            }
            if (produtoTO.Cadastro == default(DateTime))
            {
                return "O produto deve conter uma data válida.";
            }
            if (string.IsNullOrEmpty(produtoTO.Tipo.Descricao))
            {
                return "O produto deve conter um tipo.";
            }
            return null;
        }

        public void Inserir(ProdutoTO produtoTO, out string mensagem) 
        {
            mensagem = "";

            var produto = produtoTO.ConverterEmEntidade();

            using (var transaction = context.Database.BeginTransaction()) 
            {
                try
                {
                    var produtoJaExiste = context.Produtos.Where(p => p.Descricao.ToLower().Contains(produtoTO.Descricao.ToLower())).FirstOrDefault();
                    if (produtoJaExiste != null) 
                    {
                        produtoTO.Id = produtoJaExiste.Id;
                        mensagem = "Produto já foi criado.";

                        return;
                    }

                    var tipoProduto = context.TiposProdutos.Where(t => t.Descricao.ToLower().Contains(produtoTO.Tipo.Descricao.ToLower())).FirstOrDefault();
                    if (tipoProduto != null)  produto.TipoProduto = tipoProduto;
                    else context.TiposProdutos.Add(produto.TipoProduto);

                    context.Produtos.Add(produto);
                    context.SaveChanges();

                    produtoTO.Id = produto.Id;

                    transaction.Commit();
                    mensagem = "Produto criado com sucesso.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    mensagem = $"Falha ao inserir o produto. Motivo - {ex.Message}";

                    Logar(ex, mensagem);
                }
            }
        }

        public void Atualizar(ProdutoTO produtoTO, out string mensagem)
        {
            mensagem = "";
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var produto = context.Produtos.Where(p => p.Id == produtoTO.Id).FirstOrDefault();
                    if (produto == null)
                    {
                        mensagem = "Produto inválido ou inexistente.";
                        return;
                    }

                    var tipoProduto = context.TiposProdutos.Where(t => t.Descricao.ToLower().Contains(produtoTO.Tipo.Descricao.ToLower()) || t.Id == produtoTO.Tipo.Id).FirstOrDefault();
                    if (tipoProduto != null)
                    {
                        tipoProduto.Descricao = (string.IsNullOrEmpty(produtoTO.Tipo.Descricao) || produtoTO.Tipo.Descricao == "string") ? tipoProduto.Descricao : produtoTO.Tipo.Descricao;
                        produto.TipoProduto = tipoProduto;
                    } 
                    else context.TiposProdutos.Add(produto.TipoProduto);

                    produto.Descricao = produtoTO.Descricao;
                    produto.Cadastro = produtoTO.Cadastro;
                    produto.Quantidade = produtoTO.Quantidade;
                    produto.Valor = produtoTO.Valor;

                    context.Produtos.Update(produto);
                    context.SaveChanges();

                    transaction.Commit();
                    mensagem = "Produto atualizado com sucesso.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    mensagem = $"Falha ao atualizar produto. Motivo - {ex.Message}";

                    Logar(ex, mensagem);
                }
            }
            
        }

        public string Excluir(int produtoId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var produto = context.Produtos.Where(p => p.Id == produtoId).FirstOrDefault();
                    if (produto == null)
                    {
                        return "Produto inválido ou inexistente.";
                    }

                    context.Produtos.Remove(produto);
                    context.SaveChanges();

                    transaction.Commit();
                    return "Produto excluído com sucesso.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var mensagem = $"Falha ao excluir produto. Motivo - {ex.Message}";

                    Logar(ex, mensagem);
                    return mensagem;
                }
            }
        }

        private void Logar(Exception exception, string mensagem) 
        {
            _logger.Info(mensagem);
            _logger.Warn("NLOG - Erro");
            _logger.Error(exception, exception.Message);
        }

        public bool ProdutoExiste(int produtoId)
        {
            return context.Produtos.Any(p => p.Id == produtoId);
        }
    }
}
