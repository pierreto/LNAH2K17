namespace AirHockeyServer.Entities
{
    public class LoginMessage : Entity
    {
        public string username { get; set; }
        public string password { get; set; }

        public LoginMessage()
        {
        }
    }
}