using CWIProdutosAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace CWIProdutosAPI.Data
{
    public class CWIProdutosContext : DbContext
    {
        public CWIProdutosContext()
        {
        }

        public CWIProdutosContext(DbContextOptions<CWIProdutosContext> options) : base(options)
        {
        
        }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<TipoProduto> TiposProdutos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
                .Property(p => p.Descricao)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Produto>()
                .Property(p => p.Valor)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<TipoProduto>()
                .HasData(
                    new TipoProduto() { Id = 1, Descricao = "Bebida" }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ProdutosContext");
            }
        }
    }
}
