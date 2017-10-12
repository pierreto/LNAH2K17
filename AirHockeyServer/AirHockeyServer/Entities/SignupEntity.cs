namespace AirHockeyServer.Entities
{
    public class SignupEntity : Entity
    {
        public string username { get; set; }
        public string password { get; set; }

        public SignupEntity()
        {
        }
    }
}