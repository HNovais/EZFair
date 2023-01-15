using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using EZFair.Class;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace EZFair.Pages
{
    [Authorize(Roles = "Empresa")]
    public class CriarFeiraModel : PageModel
    {
        SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        [BindProperty]
        public string Nome { get; set; }
        
        [BindProperty]
        public DateTime DataInicio { get; set; }
        
        [BindProperty]
        public DateTime DataFinal { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            int id = LastID();
            id++;
            
            if (NomeEmUso(Nome) == 1) { TempData["ErrorMessage"] = "Nome já está em uso"; return null; } //ERRO NÂO TA A APARECER
            if (DataErrada(DataInicio, DataFinal) == 0) { TempData["ErrorMessage"] = "Datas estão erradas"; return null; }
           
            Feira newFeira = new Feira(id, 1, 1, Nome, 0, DataInicio, DataFinal, " ");

            await CriarFeira(newFeira);

            return Redirect("/");
        }

        private async Task CriarFeira(Feira feira)
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO Feira (idFeira, empresa, categoria, nomeFeira, numParticipantes, dataInicio, dataFim, descricao) VALUES (@idFeira, @empresa, @categoria, @nomeFeira, @numParticipantes, @dataInicio, @dataFinal, @descricao)", connection))
            {
                // Add the parameters and their values
                command.Parameters.AddWithValue("@idFeira", feira.idFeira);
                command.Parameters.AddWithValue("@empresa", feira.empresa);
                command.Parameters.AddWithValue("@categoria", feira.categoria);
                command.Parameters.AddWithValue("@nomeFeira", feira.nomeFeira);
                command.Parameters.AddWithValue("@numParticipantes", feira.numParticipantes);
                command.Parameters.AddWithValue("@dataInicio", feira.dataInicio);
                command.Parameters.AddWithValue("@dataFinal", feira.dataFinal);
                command.Parameters.AddWithValue("@descricao", feira.descricao);

                // Open the connection and execute the query
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        private int LastID()
        {
            // Open the connection
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT MAX(idFeira) AS LastID FROM Feira;", connection))
            {
                int lastId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return lastId;
            }
        }
        private int NomeEmUso(string Nome)
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT CASE WHEN EXISTS (SELECT * FROM Feira WHERE nomeFeira = @Nome) THEN 1 ELSE 0 END;", connection))
            {
                command.Parameters.AddWithValue("@Nome", Nome);

                int response = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return response;
            }
        }

        private int DataErrada(DateTime Inicio, DateTime Fim)
        {
            if (Inicio >= Fim)
                return 0;
            else
                return 1;
        }
    }
}
