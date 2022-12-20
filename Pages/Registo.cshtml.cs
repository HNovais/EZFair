using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EZFair.Class;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace EZFair.Pages
{
    public class RegistoModel : PageModel
    {
        //private readonly IConfiguration _config;

        //public void MyController(IConfiguration config)
        //{
        //    _config = config;
        //}

        //string connectionString = _config.GetConnectionString("AZURE_SQL_CONNECTION");

        public string ConnectionString { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        // Construtor do ConnectionString
        //public void UrlGetter()
        //{
        //    var builder = new ConfigurationBuilder()
        //         .SetBasePath(Directory.GetCurrentDirectory())
        //         .AddJsonFile("appsettings.json");

        //    var configuration = builder.Build();

        //    string connectionString = configuration.GetConnectionString("AZURE_SQL_CONNECTION");
        //    this.ConnectionString = connectionString;
        ////}

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int id = LastID();
            id++;

            Cliente newCliente = new Cliente(id, Name, Email, Username, Password, PhoneNumber);
            
            await RegisterCliente(newCliente);


            return RedirectToPage("Index");
        }

        private async Task RegisterCliente(Cliente cliente)
        {
            //string configJson = File.ReadAllText("appsettings.json");
            //JObject config = JObject.Parse(configJson);
            //string connectionString = (string)config["ConnectionStrings"]["AZURE_SQL_CONNECTION"];

            using (SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                // Open the connection
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO Cliente (idCliente, nome, email, username, password, numTelemovel) VALUES (@idCliente, @nome, @email, @username, @password, @numTelemovel)", connection))
                {
                    // Add the parameters and their values
                    command.Parameters.AddWithValue("@idCliente", cliente.idCliente);
                    command.Parameters.AddWithValue("@nome", cliente.nome);
                    command.Parameters.AddWithValue("@email", cliente.email);
                    command.Parameters.AddWithValue("@username", cliente.username);
                    command.Parameters.AddWithValue("@password", cliente.password);
                    command.Parameters.AddWithValue("@numTelemovel", cliente.numTelemovel);

                    // Open the connection and execute the query
                    command.ExecuteNonQuery();
                    Console.WriteLine("Done");
                }

                connection.Close();
            }
        }

        private int LastID() 
        {
            using (SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                // Open the connection
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT MAX(idCliente) AS LastID FROM Cliente;", connection))
                {
                    int lastId = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                    return lastId;
                }  
            }
        }

        /*
        public void test(string nome, string password)
        {
            string newNome = nome;
            string newPassword = password;

            Console.WriteLine(newPassword);
        }
        public string saveUserInformation(Cliente cliente)
        {
            // Store the form data in the model class
            // You can access the form data using the properties of the model class,
            // such as model.Name, model.Username, model.Email, etc.

            // You can then save the data to a database, for example, or perform some other action with it

            using (SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                // Open the connection
                connection.Open();

                // Create a command to execute the query
                SqlCommand command = new SqlCommand("INSERT INTO Cliente (idCliente, nome, email, username, password, numTelemovel)\r\nVALUES (5, {cliente.nome}, {cliente.email}, {cliente.username}, {cliente.password},{cliente.phone})\r\n", connection);

                command.ExecuteNonQuery();

            }

            return "nothing";
        }

        private System.Web.Mvc.ActionResult View()
        {
            throw new NotImplementedException();
        }

        public string getCliente()
        {
            using (SqlConnection connection = new SqlConnection("Server=tcp:ezfair.database.windows.net,1433;Initial Catalog=EZFair;Persist Security Info=False;User ID=ezfair;Password=LI4-muitofixe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                // Open the connection
                connection.Open();

                string sql = "SELECT * FROM cliente WHERE id = 5";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    reader.Read();

                    return reader.GetString(3);
                }
            }
        }*/
    }
}
