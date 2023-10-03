namespace blogPessoal.Security
{
    public class Settings
    {

        private static string secret = "f7396a530cff306ff9645b7594b32836970caa2c61b5fae8f4180d5f786d7df0";

        public static string Secret { get => secret; set => secret = value; }
    
    }

}
