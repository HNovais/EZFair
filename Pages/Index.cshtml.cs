using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EZFair.Class;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Plugins;

namespace EZFair.Pages
{
    public class IndexModel : PageModel
    {
        SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        private readonly ILogger<IndexModel> _logger;

        public static List<Feira> feiras = new List<Feira>();

        public int idEmpresa { get; set; }
        public string empresa { get; set; }
        public int idCategoria { get; set; }
        public string categoria { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            feiras.Clear();
            getFeiras();
        }

        private void getFeiras()
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                command.CommandText = "SELECT empresa, nomeFeira, dataInicio, dataFim FROM Feira";
                command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idEmpresa = reader.GetInt32(0);
                        string nomeFeira = reader.GetString(1);
                        DateTime inicio = reader.GetDateTime(2);
                        DateTime fim = reader.GetDateTime(3);

                        Feira newFeira = new Feira(idEmpresa, nomeFeira, inicio, fim);
                        feiras.Add(newFeira);
                    }
                }
            }
        }

        public void getEmpresaNome(Feira feira)
        {
            int res;
            string result = "";
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                // First query
                command.CommandText = "SELECT empresa FROM Feira WHERE nomeFeira = @feira.nomeFeira";
                command.Parameters.AddWithValue("@nomeFeira", feira.nomeFeira);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = reader.GetInt32(0);
                    }

                    reader.Close();

                    using (SqlCommand command2 = new SqlCommand("SELECT nomeEmpresa FROM Empresa WHERE idEmpresa = @res", connection))
                    {
                        command2.Parameters.AddWithValue("@res", idEmpresa);
                        using (SqlDataReader reader2 = command2.ExecuteReader())
                        {
                            if (reader2.Read())
                            {
                                result = reader2.GetString(0);
                            }
                        }
                        reader.Close();
                    }
                }
            }
            this.empresa = result;
        }


        public void getCategoriaNome(Feira feira)
        {
            int res;
            string result = "";
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                // First query
                command.CommandText = "SELECT categoria FROM Feira WHERE nomeFeira = @feira.nomeFeira";
                command.Parameters.AddWithValue("@nomeFeira", feira.nomeFeira);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = reader.GetInt32(0);
                    }

                    reader.Close();

                    using (SqlCommand command2 = new SqlCommand("SELECT nomeCategoria FROM Categoria WHERE idCategoria = @res", connection))
                    {
                        command2.Parameters.AddWithValue("@res", idCategoria);
                        using (SqlDataReader reader2 = command2.ExecuteReader())
                        {
                            if (reader2.Read())
                            {
                                result = reader2.GetString(0);
                            }
                        }
                        reader.Close();
                    }
                }
            }
            this.categoria = result;
        }
    }
}