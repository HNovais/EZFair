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

        private int idFeira;
        public string nomeFeira { get; set; }
        public string empresa { get; set; }
        private int idEmpresa { get; set; }
        public DateTime inicio { get; set; }
        public DateTime final { get; set; }

        public Feira feira;
        
        public void OnGet(string nomeFeira)
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT empresa, dataInicio, dataFim FROM Feira WHERE nomeFeira = @nomeFeira", connection))
            {
                command.Parameters.AddWithValue("@nomeFeira", nomeFeira);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.idEmpresa = reader.GetInt32(0);
                        this.nomeFeira = nomeFeira;
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
            }
        }

        public IActionResult AdicionarProduto()
        {
            // Realizar as operações de registro aqui.

            return RedirectToAction("/Feira" + nomeFeira + "/AdicionarProduto", nomeFeira, idFeira);
        }
    }
}
