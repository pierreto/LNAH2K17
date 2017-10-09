using System.ComponentModel.DataAnnotations.Schema;

namespace AirHockeyServer.Pocos
{
    [System.Data.Linq.Mapping.Table(Name = "test_passwords")]
    public class PasswordPoco : Poco
    {
        [System.Data.Linq.Mapping.Column(IsPrimaryKey = true, Name = "id_password")]
        public int Id { get; private set; }
        
        [ForeignKey("UserPoco"), System.Data.Linq.Mapping.Column(Name = "id_user")]
        public int UserId { get; private set; }

        [System.Data.Linq.Mapping.Column(Name = "password")]
        public string Password { get; private set; }

        public UserPoco User { get; set; }
    }
}


