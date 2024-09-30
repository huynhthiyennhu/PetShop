using System.Text;

namespace PetShop.Helpers
{
    public class MyUtil
    {
        public static string GenerateRandomKey(int leght=5) 
        {
            var pattern = @"zxcvbnm!";
            var sb =new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < leght; i++) 
            {
                sb.Append(pattern[rd.Next(0,pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}
