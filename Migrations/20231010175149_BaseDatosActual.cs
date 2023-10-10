using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen_UII.Migrations
{
    /// <inheritdoc />
    public partial class BaseDatosActual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Usuarios_UsuariosID",
                table: "Dispositivos");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Zonas_ZonasID",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "Dispositivos");

            migrationBuilder.RenameColumn(
                name: "ZonasID",
                table: "Dispositivos",
                newName: "ZonasId");

            migrationBuilder.RenameColumn(
                name: "UsuariosID",
                table: "Dispositivos",
                newName: "UsuariosId");

            migrationBuilder.RenameColumn(
                name: "IdUsuarios",
                table: "Dispositivos",
                newName: "EstadosId");

            migrationBuilder.RenameIndex(
                name: "IX_Dispositivos_ZonasID",
                table: "Dispositivos",
                newName: "IX_Dispositivos_ZonasId");

            migrationBuilder.RenameIndex(
                name: "IX_Dispositivos_UsuariosID",
                table: "Dispositivos",
                newName: "IX_Dispositivos_UsuariosId");

            migrationBuilder.AddColumn<int>(
                name: "MunicipiosId",
                table: "Zonas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UsuariosId",
                table: "Dispositivos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zonas_MunicipiosId",
                table: "Zonas",
                column: "MunicipiosId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_EstadosId",
                table: "Dispositivos",
                column: "EstadosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Estados_EstadosId",
                table: "Dispositivos",
                column: "EstadosId",
                principalTable: "Estados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Usuarios_UsuariosId",
                table: "Dispositivos",
                column: "UsuariosId",
                principalTable: "Usuarios",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Zonas_ZonasId",
                table: "Dispositivos",
                column: "ZonasId",
                principalTable: "Zonas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zonas_Municipios_MunicipiosId",
                table: "Zonas",
                column: "MunicipiosId",
                principalTable: "Municipios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Estados_EstadosId",
                table: "Dispositivos");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Usuarios_UsuariosId",
                table: "Dispositivos");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Zonas_ZonasId",
                table: "Dispositivos");

            migrationBuilder.DropForeignKey(
                name: "FK_Zonas_Municipios_MunicipiosId",
                table: "Zonas");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropIndex(
                name: "IX_Zonas_MunicipiosId",
                table: "Zonas");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_EstadosId",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "MunicipiosId",
                table: "Zonas");

            migrationBuilder.RenameColumn(
                name: "ZonasId",
                table: "Dispositivos",
                newName: "ZonasID");

            migrationBuilder.RenameColumn(
                name: "UsuariosId",
                table: "Dispositivos",
                newName: "UsuariosID");

            migrationBuilder.RenameColumn(
                name: "EstadosId",
                table: "Dispositivos",
                newName: "IdUsuarios");

            migrationBuilder.RenameIndex(
                name: "IX_Dispositivos_ZonasId",
                table: "Dispositivos",
                newName: "IX_Dispositivos_ZonasID");

            migrationBuilder.RenameIndex(
                name: "IX_Dispositivos_UsuariosId",
                table: "Dispositivos",
                newName: "IX_Dispositivos_UsuariosID");

            migrationBuilder.AlterColumn<int>(
                name: "UsuariosID",
                table: "Dispositivos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Estatus",
                table: "Dispositivos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Usuarios_UsuariosID",
                table: "Dispositivos",
                column: "UsuariosID",
                principalTable: "Usuarios",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Zonas_ZonasID",
                table: "Dispositivos",
                column: "ZonasID",
                principalTable: "Zonas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
