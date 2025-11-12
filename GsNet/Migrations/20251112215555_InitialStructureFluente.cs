using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GsNet.Migrations
{
    /// <inheritdoc />
    public partial class InitialStructureFluente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SEU_SCHEMA_AQUI");

            migrationBuilder.CreateTable(
                name: "TB_GS_LOCALIZACAO",
                schema: "SEU_SCHEMA_AQUI",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TIPO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    GRAUS_CELCIUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NIVEL_UMIDADE = table.Column<float>(type: "BINARY_FLOAT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GS_LOCALIZACAO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_GS_LOGIN",
                schema: "SEU_SCHEMA_AQUI",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GS_LOGIN", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_GS_USUARIO",
                schema: "SEU_SCHEMA_AQUI",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ID_LOCAL_TRABALHO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GS_USUARIO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USUARIO_LOCAL",
                        column: x => x.ID_LOCAL_TRABALHO,
                        principalSchema: "SEU_SCHEMA_AQUI",
                        principalTable: "TB_GS_LOCALIZACAO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_GS_MENSAGEM",
                schema: "SEU_SCHEMA_AQUI",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MENSAGEM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NIVEL_ESTRESSE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_USUARIO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GS_MENSAGEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MENSAGEM_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalSchema: "SEU_SCHEMA_AQUI",
                        principalTable: "TB_GS_USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_GS_MENSAGEM_ID_USUARIO",
                schema: "SEU_SCHEMA_AQUI",
                table: "TB_GS_MENSAGEM",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_GS_USUARIO_ID_LOCAL_TRABALHO",
                schema: "SEU_SCHEMA_AQUI",
                table: "TB_GS_USUARIO",
                column: "ID_LOCAL_TRABALHO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_GS_LOGIN",
                schema: "SEU_SCHEMA_AQUI");

            migrationBuilder.DropTable(
                name: "TB_GS_MENSAGEM",
                schema: "SEU_SCHEMA_AQUI");

            migrationBuilder.DropTable(
                name: "TB_GS_USUARIO",
                schema: "SEU_SCHEMA_AQUI");

            migrationBuilder.DropTable(
                name: "TB_GS_LOCALIZACAO",
                schema: "SEU_SCHEMA_AQUI");
        }
    }
}
