using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using AirHockeyServer.Entities.EditionCommand;

namespace AirHockeyServer.Hubs
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file ConnectionMapper.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe permet de gérer les connections des clients. Il est ainsi possible 
    /// de savoir quelle connection utiliser pour envoyer un événement à un utilisateur
    /// spécifique. Les propritétés sont statiques pour que les "Hubs" puissent
    /// accéder aux connections de manière centralisée.
    ///////////////////////////////////////////////////////////////////////////////
    public class ConnectionMapper
    {
        private ConcurrentDictionary<int, string> _ConnectionsMapping;
        private ConcurrentDictionary<int, string> ConnectionsMapping
        {
            get
            {
                if (_ConnectionsMapping == null)
                {
                    _ConnectionsMapping = new ConcurrentDictionary<int, string>();
                }
                return _ConnectionsMapping;
            }
            set
            {
                _ConnectionsMapping = value;
            }
        }

        private ConcurrentDictionary<string, Guid> _GameID;
        private  ConcurrentDictionary<string, Guid> GameID
        {
            get
            {
                if (_GameID == null)
                    _GameID = new ConcurrentDictionary<string, Guid>();
                return _GameID;
            }
            set { _GameID = value; }
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn static bool AddConnection(Guid userId, string connection)
        ///
        /// Cette fonction permet l'ajout d'une connection
        ///
        /// @return true si la connection a été ajoutée
        ///
        ////////////////////////////////////////////////////////////////////////
        public bool AddConnection(int userId, string connection)
        {
            if (!ConnectionsMapping.ContainsKey(userId))
            {
                ConnectionsMapping[userId] = connection;
                return true;
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn static string GetConnection(Guid userId)
        ///
        /// Cette fonction permet le récupérer une connection à partir
        /// du Id d'un utilisateur
        ///
        /// @return retourne la connection 
        ///
        ////////////////////////////////////////////////////////////////////////
        public string GetConnection(int userId)
        {
            if (ConnectionsMapping.ContainsKey(userId))
            {
                return ConnectionsMapping[userId];
            }
            return string.Empty;
        }

        public void AddGameID(string connection, Guid gameID)
        {
            GameID[connection] = gameID;
        }

        public Guid GetGameId(string connection)
        {
            return GameID[connection];
        }

        private ConcurrentDictionary<string, OnlineUser> usersConnectionMapping;
        private ConcurrentDictionary<string, OnlineUser> UsersConnectionMapping
        {
            get
            {
                return usersConnectionMapping ??
                       (usersConnectionMapping = new ConcurrentDictionary<string, OnlineUser>());
            }
            set => usersConnectionMapping = value;
        }

        public void AddUserConnection(string connectionId, OnlineUser User)
        {
            UsersConnectionMapping[connectionId] = User;
        }

        public void RemoveUserConnection(string connectionId)
        {
            ((IDictionary)UsersConnectionMapping).Remove(connectionId);
        }

        public OnlineUser GetUserFromConnectionId(string connectionId)
        {
            return UsersConnectionMapping[connectionId];
        }

        public void DeleteConnection(int userId)
        {
            if (ConnectionsMapping.ContainsKey(userId))
            {
                string connectionRemoved = "";
                ConnectionsMapping.TryRemove(userId, out connectionRemoved);
            }
            
        }
    }
}