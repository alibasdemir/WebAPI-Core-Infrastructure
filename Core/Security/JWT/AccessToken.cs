namespace Core.Security.JWT
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime TokenExpirationTime { get; set; }
    }
}
