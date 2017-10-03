namespace AirHockeyServer.Entities
{
    public class SignupMessage : Entity
    {
        public string username { get; set; }
        public string password { get; set; }

        public SignupMessage()
        {
        }
    }
}