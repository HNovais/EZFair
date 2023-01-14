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

        private static int produto;
        private static string feira;
        private static Produto prod{ get; set; }

        public void OnGet(string nomeFeira, int idProduto)
        {
            ComprarProdutoModel.feira = nomeFeira;
            ComprarProdutoModel.produto = idProduto;
            Console.WriteLine(nomeFeira + idProduto);
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
                command.Parameters.AddWithValue("@idProduto", produto);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int stock = reader.GetInt32(0);
                        double temp = reader.GetDouble(1);
                        float preco = (float)temp;
                        string nomeProduto = reader.GetString(2);

                        prod = new Produto(produto, stock, preco, nomeProduto);
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
                command.Parameters.AddWithValue("@idProduto", produto);
                command.ExecuteNonQuery(); 

                // Second query
                command.CommandText = "DELETE FROM Produto WHERE idProduto = @prod;";
                command.Parameters.AddWithValue("@prod", produto);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
