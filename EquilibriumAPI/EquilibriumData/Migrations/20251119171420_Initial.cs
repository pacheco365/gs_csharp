using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquilibriumData.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuariosGS",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SaldoEQ = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosGS", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "TransacaoGS",
                columns: table => new
                {
                    IdTransacao = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Valor = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataTransacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UsuarioId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransacaoGS", x => x.IdTransacao);
                    table.ForeignKey(
                        name: "FK_TransacaoGS_UsuariosGS_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuariosGS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransacaoGS_UsuarioId",
                table: "TransacaoGS",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosGS_Email",
                table: "UsuariosGS",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransacaoGS");

            migrationBuilder.DropTable(
                name: "UsuariosGS");
        }
    }
}
