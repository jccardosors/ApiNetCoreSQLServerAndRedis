using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_ENTRY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValueEntry = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateEntry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCredit = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ENTRY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_LOGS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LOGS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SENHA = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO_CTRL_ACESSO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LAST_ACCESS = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    TRY_NUMBER = table.Column<int>(type: "int", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false),
                    USER_EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO_CTRL_ACESSO", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TB_USUARIO",
                columns: new[] { "Id", "Email", "SENHA" },
                values: new object[] { 1, "teste@gmail.com", "senha123" });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ENTRY_Id",
                table: "TB_ENTRY",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_LOGS_Id",
                table: "TB_LOGS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_Id",
                table: "TB_USUARIO",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_CTRL_ACESSO_Id",
                table: "TB_USUARIO_CTRL_ACESSO",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ENTRY");

            migrationBuilder.DropTable(
                name: "TB_LOGS");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_USUARIO_CTRL_ACESSO");
        }
    }
}
