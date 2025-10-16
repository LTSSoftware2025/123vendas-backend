using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _123Vendas.Infra.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "venda",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    numero_venda = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    data_efetuacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_cliente_externo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome_cliente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    id_filial_externa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome_filial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    cancelada = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venda", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_venda",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_venda = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_produto_externo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    produto_descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    valor_unitario_produto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    desconto_percentual = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_venda", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_venda_venda_id_venda",
                        column: x => x.id_venda,
                        principalTable: "venda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_id_venda",
                table: "item_venda",
                column: "id_venda");

            migrationBuilder.CreateIndex(
                name: "IX_venda_data_efetuacao",
                table: "venda",
                column: "data_efetuacao");

            migrationBuilder.CreateIndex(
                name: "IX_venda_id_cliente_externo",
                table: "venda",
                column: "id_cliente_externo");

            migrationBuilder.CreateIndex(
                name: "IX_venda_id_filial_externa",
                table: "venda",
                column: "id_filial_externa");

            migrationBuilder.CreateIndex(
                name: "IX_venda_numero_venda",
                table: "venda",
                column: "numero_venda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_venda");

            migrationBuilder.DropTable(
                name: "venda");
        }
    }
}
