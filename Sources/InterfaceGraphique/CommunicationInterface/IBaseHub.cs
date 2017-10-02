﻿using System;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    public interface IBaseHub
    {
        void InitializeHub(HubConnection connection, string username);
        void Logout();
    }
}