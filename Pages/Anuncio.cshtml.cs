using EZFair.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;

namespace EZFair.Pages
{
    public class AnuncioModel : PageModel
    {

        SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        public string descricao { get; set; }
        public static int produto { get; set; } 
        public static string nome { get; set; }
        // bytes de images
        

        public static float preco { get; set; }

        private static string feira;
        public void OnGet(string nomeFeira, int idProduto)
        {
            AnuncioModel.produto = idProduto;
            AnuncioModel.feira = nomeFeira;

            getProduto();
        }
       
        private void getProduto()
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                // First query
                command.CommandText = "SELECT idFeira FROM Feira WHERE nomeFeira = @nomeFeira";
                command.Parameters.AddWithValue("@nomeFeira", feira);
                int idFeira = (int)command.ExecuteScalar();

                // Second query
                command.CommandText = "SELECT descricao FROM Anuncio WHERE feira = @idFeira";
                command.Parameters.AddWithValue("@idFeira", idFeira);
                descricao = command.ExecuteScalar() as string;

                //Third query
                command.CommandText = "SELECT preco, nomeProduto FROM Produto WHERE idProduto = @produto";
                command.Parameters.AddWithValue("@produto", produto);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        double temp = reader.GetDouble(0);
                        preco = Convert.ToSingle(temp);
                        nome = reader.GetString(1);
                    }
                }
            }

            connection.Close();

        }
        /*
        private void putImage()
        {
            //string connectionString = "Data Source=(local);Initial Catalog=myDB;Integrated Security=True;";
            byte[] imageData = File.ReadAllBytes("Data/Images/my-nba-all-stars-picks-story.png");
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                command.CommandText = "INSERT INTO Images (ImageData) VALUES (@imageData)";
                //var command = new SqlCommand(query, connection);
                command.Parameters.Add("@imageData", imageData);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        */
        public IActionResult OnPostComprar()
        {
            return RedirectToPage("ComprarProduto", new { feira, produto });
        }
    }
}

