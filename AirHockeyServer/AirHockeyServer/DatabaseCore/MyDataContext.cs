using AirHockeyServer.DatabaseCore;
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

    public Table<UserPoco> UsersTable
    {
        get { return this.GetTable<UserPoco>(); }
    }

    public Table<FriendPoco> FriendsTable
    {
        get { return this.GetTable<FriendPoco>(); }
    }

    public Table<MapPoco> MapsTable
    {
        get { return this.GetTable<MapPoco>(); }
    }
}