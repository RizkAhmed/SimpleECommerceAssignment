namespace ECommerce.Business.Options
{
    public class JwtOptions
    {
        public static string SectionName { get; set; } = "Jwt";
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }

}
