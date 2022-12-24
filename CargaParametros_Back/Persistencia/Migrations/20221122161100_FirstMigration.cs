using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistencia.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parametro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VcNombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VcCodigoInterno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BEstado = table.Column<bool>(type: "bit", nullable: false),
                    DtFechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtFechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DtFechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParametroDetalle",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParametroId = table.Column<long>(type: "bigint", nullable: false),
                    VcNombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TxDescripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdPadre = table.Column<long>(type: "bigint", nullable: true),
                    VcCodigoInterno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DCodigoIterno = table.Column<decimal>(type: "decimal(17,3)", precision: 17, scale: 3, nullable: true),
                    BEstado = table.Column<bool>(type: "bit", nullable: false),
                    RangoDesde = table.Column<int>(type: "int", nullable: true),
                    RangoHasta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametroDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parametro",
                        column: x => x.ParametroId,
                        principalTable: "Parametro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParametroDetalle_ParametroId",
                table: "ParametroDetalle",
                column: "ParametroId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParametroDetalle");

            migrationBuilder.DropTable(
                name: "Parametro");
        }
    }
}
