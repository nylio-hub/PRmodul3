using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;




namespace WpfApp1.Services
{
    public class Hash
    {
        
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceBytePassword = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256Hash.ComputeHash(sourceBytePassword);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}