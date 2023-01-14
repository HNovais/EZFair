using EZFair.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EZFair.Pages
{
    public class ComprarProdutoModel : PageModel
    {
        SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        private static int idProduto;
        private static string nomeFeira;
        private static Produto produto { get; set; }

        public void OnGet(string feira, int produto)
        {
            ComprarProdutoModel.nomeFeira = feira;
            ComprarProdutoModel.idProduto = produto;

            getProduto();
        }

        private void getProduto()
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                // First query
                command.CommandText = "SELECT stock, preco, nomeProduto FROM Produto WHERE idProduto = @idProduto";
                command.Parameters.AddWithValue("@idProduto", idProduto);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int stock = reader.GetInt32(0);
                        double temp = reader.GetDouble(1);
                        float preco = (float)temp;
                        string nomeProduto = reader.GetString(2);

                        produto = new Produto(idProduto, stock, preco, nomeProduto);
                    }
                }
            }

            connection.Close();
        }

        [HttpPost]
        public void OnPostConfirmarCompra()
        {
            deleteProduto();
        }

        private void deleteProduto()
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                // First query
                command.CommandText = "DELETE FROM Anuncio WHERE produto = @idProduto;";
                command.Parameters.AddWithValue("@idProduto", idProduto);
                command.ExecuteNonQuery(); 

                // Second query
                command.CommandText = "DELETE FROM Produto WHERE idProduto = @prod;";
                command.Parameters.AddWithValue("@prod", idProduto);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
