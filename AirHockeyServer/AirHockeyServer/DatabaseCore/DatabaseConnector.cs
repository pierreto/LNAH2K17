using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.DatabaseCore
{
    public class DatabaseConnector
    {
        private const string hostname = "37.187.19.181";
        private DatabaseConnector()
        {
        }

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DatabaseConnector _instance = null;
        public static DatabaseConnector Instance()
        {
            if (_instance == null)
                _instance = new DatabaseConnector();
            return _instance;
        }

        public bool IsConnect()
        {
            bool result = true;
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    result = false;
                string connstring = string.Format("Server=" + hostname + "; database={0}; UID=log3900; password=labasedesdecales", databaseName);
                connection = new MySqlConnection(connstring);
                connection.Open();
                result = true;
            }

            return result;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}