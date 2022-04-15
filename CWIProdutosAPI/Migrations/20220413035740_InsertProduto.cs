using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CWIProdutosAPI.Migrations
{
    public partial class InsertProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Cadastro", "Descricao", "Quantidade", "Tipo", "Valor" },
                values: new object[] { 1, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "Caneta", 2, 4, 2.70m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
