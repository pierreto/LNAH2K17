using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Dictionary<Guid, string> _ConnectionsMapping;
        private static Dictionary<Guid, string> ConnectionsMapping
        {
            get
            {
                if (_ConnectionsMapping == null)
                {
                    _ConnectionsMapping = new Dictionary<Guid, string>();
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
            if (!ConnectionsMapping.ContainsKey(userId))
            {
                ConnectionsMapping.Add(userId, connection);
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
        public static string GetConnection(Guid userId)
        {
            if (ConnectionsMapping.ContainsKey(userId))
            {
                return ConnectionsMapping[userId];
            }
            return string.Empty;
        }
    }
}