using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

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
        private static Mutex ConnectionMutex = new Mutex();

        private static ConcurrentDictionary<Guid, string> _ConnectionsMapping;
        private static ConcurrentDictionary<Guid, string> ConnectionsMapping
        {
            get
            {
                if (_ConnectionsMapping == null)
                {
                    _ConnectionsMapping = new ConcurrentDictionary<Guid, string>();
                }
                return _ConnectionsMapping;
            }
            set
            {
                _ConnectionsMapping = value;
            }
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
        public static bool AddConnection(Guid userId, string connection)
        {
            //ConnectionMutex.WaitOne();

            if (!ConnectionsMapping.ContainsKey(userId))
            {
                ConnectionsMapping[userId] = connection;
                return true;
            }

            //ConnectionMutex.ReleaseMutex();

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
        public static string GetConnection(Guid userId)
        {
            //ConnectionMutex.WaitOne();

            if (ConnectionsMapping.ContainsKey(userId))
            {
                return ConnectionsMapping[userId];
            }

            //ConnectionMutex.ReleaseMutex();

            return string.Empty;
        }
    }
}