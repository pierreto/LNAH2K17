using AirHockeyServer.Pocos;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.Linq;

public class MyDataContext : DataContext
{
    public MyDataContext() : base(new MySqlConnection(ConfigurationManager.ConnectionStrings["lnah"].ConnectionString)) {
    }

    public Table<UserPoco> Users
    {
        get { return this.GetTable<UserPoco>(); }
    }

    public Table<PasswordPoco> Passwords
    {
        get { return this.GetTable<PasswordPoco>(); }
    }
}