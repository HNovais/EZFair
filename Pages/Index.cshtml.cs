using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EZFair.Class;
using Microsoft.Data.SqlClient;

namespace EZFair.Pages
{
    public class IndexModel : PageModel
    {
        SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        private readonly ILogger<IndexModel> _logger;
        
        public static List<Feira> feiras = new List<Feira>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
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
    }
}