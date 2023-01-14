using EZFair.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace EZFair.Pages
{
    public class AdicionarProdutoModel : PageModel
    {
        SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        [BindProperty]
        public int Stock { get; set; }
        
        [BindProperty]
        public float Price { get; set; }
        
        [BindProperty]
        public string Name { get; set; }

        public string nomeFeira;
        private int idFeira;

        public IActionResult OnGet(string nomeFeira, int idFeira)
        {
            this.idFeira = idFeira;
            this.nomeFeira = nomeFeira;
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            Produto produto = new Produto(Stock, Price, Name);

            int idProduto = AdicionarProduto(produto, idFeira);

            return RedirectToPage("/Feira/" + nomeFeira + "/" + idProduto);
        }

        private int AdicionarProduto(Produto produto, int idFeira)
        {
            int idProduto;
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                // First query
                command.CommandText = "INSERT INTO Produto (stock, preco, nomeProduto) VALUES (@stock, @preco, @nome)";
                command.Parameters.AddWithValue("@stock", produto.stock);
                command.Parameters.AddWithValue("@preco", produto.preco);
                command.Parameters.AddWithValue("@nome", produto.nomeProduto);
                command.ExecuteNonQuery();

                // Second query
                command.CommandText = "SELECT IDENT_CURRENT('Produto')\r\n";
                idProduto = (int)command.ExecuteScalar();

                Anuncio anuncio = new Anuncio(idProduto, idFeira);

                //Third query
                command.CommandText = "INSERT INTO Anuncio (produto, feira) VALUES (@produto, @feira)";
                command.Parameters.AddWithValue("@produto", anuncio.produto);
                command.Parameters.AddWithValue("@feira", anuncio.feira);
                command.ExecuteNonQuery();
            }

            connection.Close();

            return idProduto;
        }

    }
}
