using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CWIProdutosAPI.Migrations
{
    public partial class CreateTableTipoProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "TipoProdutoId",
                table: "Produtos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TiposProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposProdutos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TiposProdutos",
                columns: new[] { "Id", "Descricao" },
                values: new object[] { 1, "Bebida" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_TipoProdutoId",
                table: "Produtos",
                column: "TipoProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_TiposProdutos_TipoProdutoId",
                table: "Produtos",
                column: "TipoProdutoId",
                principalTable: "TiposProdutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_TiposProdutos_TipoProdutoId",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "TiposProdutos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_TipoProdutoId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "TipoProdutoId",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Cadastro", "Descricao", "Quantidade", "Tipo", "Valor" },
                values: new object[] { 1, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "Caneta", 2, 4, 2.70m });
        }
    }
}
