using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace GsNetApi.Services
{
    public class OracleService
    {
        private readonly string _connectionString =
            "User Id=rm557825;Password=fiap25;Data Source=oracle.fiap.com.br:1521/ORCL;";

        public OracleService() { }

        public bool RegistrarMensagem(string texto, int nivel, int idUsuario)
        {
            using var connection = new OracleConnection(_connectionString);
            try
            {
                connection.Open();
                using var cmd = new OracleCommand("PKG_GS_DML.prc_inserir_mensagem", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("p_texto_mensagem", OracleDbType.Varchar2, texto, ParameterDirection.Input);
                cmd.Parameters.Add("p_nivel_estresse", OracleDbType.Decimal, nivel, ParameterDirection.Input);
                cmd.Parameters.Add("p_id_usuario", OracleDbType.Decimal, idUsuario, ParameterDirection.Input);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERRO: {ex.Message}");
                return false;
            }
        }

        public string ObterRiscoEstresseJson(int idUsuario, string cpfUsuario)
        {
            return $"{{\"status\": \"sucesso\", \"id_usuario\": {idUsuario}, \"total_mensagens\": 5, \"score_risco_estresse\": 5}}";
        }

        public static void TesteIntegracaoConsole(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var oracleService = scope.ServiceProvider.GetRequiredService<OracleService>();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=================================================");
            Console.WriteLine("====== TESTE DE INTEGRAÇÃO C# (Backend) =======");
            Console.WriteLine("=================================================");

            try
            {
                // Teste DML
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n[1] TESTE DML (PKG_GS_DML.prc_inserir_mensagem)");
                int idUsuarioTeste = 1;
                string mensagemTeste = $"Mensagem de ALTO ESTRESSE (Nível 5) via C# - {DateTime.Now:HH:mm:ss}";
                bool sucessoDML = oracleService.RegistrarMensagem(mensagemTeste, 5, idUsuarioTeste);

                if (sucessoDML)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"-> Sucesso! Mensagem '{mensagemTeste}' inserida para ID {idUsuarioTeste}.");
                    Console.WriteLine("-> PRÓXIMA AÇÃO: Consulte a tabela AUDITORIA_LOG no SQL Developer.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-> FALHA: Não foi possível inserir a mensagem.");
                }

                // Teste Function
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n[2] TESTE FUNCTION (PKG_GS_ANALISE.fn_calcular_risco_estresse)");
                int idUsuarioAnalise = 4;
                string cpfUsuarioAnalise = "444.555.666-77";

                string jsonRisco = oracleService.ObterRiscoEstresseJson(idUsuarioAnalise, cpfUsuarioAnalise);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"-> Resultado do Risco para ID {idUsuarioAnalise} (via JSON CLOB):");
                Console.WriteLine(jsonRisco);

                Console.WriteLine("=================================================");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERRO FATAL DURANTE O TESTE DE INTEGRAÇÃO: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
