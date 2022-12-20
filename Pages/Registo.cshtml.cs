using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EZFair.Class;
using Microsoft.Data.SqlClient;
using System.Drawing.Printing;

namespace EZFair.Pages
{
    public class RegistoModel : PageModel
    {
        public Cliente newCliente;

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
        }
    }
}
