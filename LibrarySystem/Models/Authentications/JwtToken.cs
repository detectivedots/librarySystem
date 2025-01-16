namespace LibrarySystem.Models.Authentications
{
    public class JwtToken
    {
        public string Token { get; set; }
        public DateTime ExpireTokenDate { get; set; }
    }
}
