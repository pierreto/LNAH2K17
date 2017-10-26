namespace AirHockeyServer.Entities
{
    public class SignupEntity : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public SignupEntity()
        {
        }
    }
}