using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cifrado.Migrations
{
    /// <inheritdoc />
    public partial class AddEncriptadoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Encriptados",
                columns: table => new
                {
                    IdEncriptado = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextEncript = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyEncript = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    btFecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encriptados", x => x.IdEncriptado);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Encriptados_KeyEncript",
                table: "Encriptados",
                column: "KeyEncript",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Encriptados");
        }
    }
}
