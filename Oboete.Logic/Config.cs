namespace Oboete.Logic
{
    public static class Config
    {
        public static string ConnectionString { get; set; }
        public static string SecretKey { get; set; }
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static string GlobalHashSalt { get; set; }
    }
}