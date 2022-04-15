using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CWIProdutosAPI.Data;
using CWIProdutosAPI.Models;
using Microsoft.Extensions.Logging;
using System;

namespace CWIProdutosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ILogger<ProdutoController> _logger;
        private static CWIProdutosContext unitOfWork = new CWIProdutosContext();
        private readonly IProdutoService _service = new ProdutoService(unitOfWork);

        public ProdutoController(ILogger<ProdutoController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os produtos.
        /// </summary>
        /// <returns></returns>
        // GET: api/Produto
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoTO>> GetProdutos()
        {
            try
            {
                return _service.Listar();
            }
            catch (Exception ex) 
            {
                _logger.LogInformation("Erro inesperado no serviço.");
                _logger.LogWarning("NLOG - Erro");
                _logger.Log(LogLevel.Error, ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Consulta o produto por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Produto/5
        [HttpGet("{id}")]
        public ActionResult<RetornoServico> GetProduto(int id)
        {
            var produtoTO = _service.Consultar(id);

            if (produtoTO == null)
            {
                return NotFound();
            }

            return produtoTO;
        }

        /// <summary>
        /// Atualiza o produto por id.
        /// </summary>
        /// <param name="id"></param>
        /// /// <param name="produtoTO"></param>
        /// <returns></returns>
        // PUT: api/Produto/5
        [HttpPut("{id}")]
        public ActionResult<RetornoServico> PutProduto(int id, ProdutoTO produtoTO)
        {
            if (id != produtoTO.Id)
            {
                return BadRequest();
            }

            try
            {
                return _service.Salvar(produtoTO);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogInformation("Erro inesperado no serviço.");
                _logger.LogWarning("NLOG - Erro");
                _logger.Log(LogLevel.Error, ex, ex.Message);

                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="produtoInputTO"></param>
        /// <returns></returns>
        // POST: api/Produto
        [HttpPost]
        public ActionResult<RetornoServico> PostProduto(ProdutoInputTO produtoInputTO)
        {
            var retorno = _service.Salvar(produtoInputTO.ConverterEmTO());
            return retorno;
        }

        /// <summary>
        /// Exclui o produto por id.
        /// </summary>
        /// <param name="produtoInputTO"></param>
        /// <returns></returns>
        // DELETE: api/Produto/5
        [HttpDelete("{id}")]
        public ActionResult<RetornoServico> DeleteProduto(int id)
        {
            var produto = _service.Consultar(id).Produto;
            if (produto == null)
            {
                return NotFound();
            }

            var mensagem = _service.Excluir(id);

            return new RetornoServico(produto, mensagem);
        }

        private bool ProdutoExists(int id)
        {
            return _service.ProdutoExiste(id);
        }
    }
}
