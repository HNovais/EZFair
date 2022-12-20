using System.ComponentModel.DataAnnotations;

namespace EZFair.Class
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public string nome { get; set; }
        public string email { get; set;}
        public string username { get; set;}
        public string password { get; set;}
        public string numTelemovel { get; set;}
    }
}
