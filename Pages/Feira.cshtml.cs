using EZFair.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Composition.Convention;
using System.Data;

namespace EZFair.Pages
{
    public class FeiraModel : PageModel
    {
        SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        private static int idFeira;
        public static string nomeFeira { get; set; }
        public string empresa { get; set; }
        private int idEmpresa { get; set; }
        public DateTime inicio { get; set; }
        public DateTime final { get; set; }

        public Feira feira;
        
        public static List<Produto> produtos = new List<Produto>();

        public void OnGet(string nomeFeira)
        {
            produtos.Clear();
            getFeira(nomeFeira);
            getAnuncios();
        }

        private void getFeira(string nomeFeira)
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                // First query
                command.CommandText = "SELECT empresa, dataInicio, dataFim FROM Feira WHERE nomeFeira = @nomeFeira";
                command.Parameters.AddWithValue("@nomeFeira", nomeFeira);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.idEmpresa = reader.GetInt32(0);
                        FeiraModel.nomeFeira = nomeFeira;
                        this.inicio = reader.GetDateTime(1);
                        this.final = reader.GetDateTime(2);
                    }

                    feira = new Feira(idEmpresa, nomeFeira, inicio, final);
                    reader.Close();

                    using (SqlCommand command2 = new SqlCommand("SELECT nomeEmpresa FROM Empresa WHERE idEmpresa = @idEmpresa", connection))
                    {
                        command2.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        using (SqlDataReader reader2 = command2.ExecuteReader())
                        {
                            if (reader2.Read())
                            {
                                this.empresa = reader2.GetString(0);
                            }
                        }
                    }
                }

                // Second query
                command.CommandText = "SELECT idFeira FROM Feira WHERE nomeFeira = @nome";
                command.Parameters.AddWithValue("@nome", nomeFeira);
                FeiraModel.idFeira = (int)command.ExecuteScalar();
            }

            connection.Close();
        }

        private void getAnuncios()
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                command.CommandText = "SELECT Produto.*\r\nFROM Feira\r\nJOIN Anuncio ON Feira.idFeira = Anuncio.feira\r\nJOIN Produto ON Anuncio.produto = Produto.idProduto\r\nWHERE Feira.idFeira = @idFeira";
                command.Parameters.AddWithValue("@idFeira", idFeira);
                command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idProduto = reader.GetInt32(0);
                        int stock = reader.GetInt32(1);
                        double temp = reader.GetDouble(2);
                        float preco = (float)temp;
                        string nomeProduto = reader.GetString(3);

                        Produto newProduto = new Produto(idProduto, stock, preco, nomeProduto);
                        produtos.Add(newProduto);
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult OnPostAdicionarProduto()
        {
            return RedirectToPage("AdicionarProduto", new { nomeFeira, idFeira });
        }
    }
}
