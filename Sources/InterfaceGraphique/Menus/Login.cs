using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique.Menus
{
    public partial class Login : Form
    {
        private Socket clientSocket;
        private Thread receiveDataThread;
        private string contentReceived;



        public Login()
        {
            InitializeComponent();


            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Mécanique de login ici
            if (this.loginName.Text == "")
            {
                MessageBox.Show(@"Le pseudo ne pas être nul!");
                return;
            }
            if (this.serverAdress.Text == "")
            {
                MessageBox.Show(@"L'adresse ip du serveur ne peut pas être nulle!");
                return;
            }
            //Chaque message sera précédé d'un numéro de sequence
            //Le numéro de séquence 1 servira à identifier le pseudo
            //cté serveur.
            IPAddress ip = IPAddress.Parse(this.serverAdress.Text);
            IPEndPoint ipEnd = new IPEndPoint(ip, 8000);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(ipEnd);
                if (clientSocket.Connected)
                {
                    this.sendMessage(this.loginName.Text);
                    this.LoginButton.Enabled = false;
                }
            }
            catch (SocketException E)
            {
                MessageBox.Show(@"La connexion a échoué : " + E.Message);
            }
            try
            {
                this.receiveDataThread = new Thread(new ThreadStart(CheckData));
                this.receiveDataThread.Start();
            }
            catch (Exception E)
            {
                MessageBox.Show("Démarrage Thread" + E.Message);
            }
        }

        private void serverAdress_TextChanged(object sender, EventArgs e)
        {

        }


        private void sendMessage(string message)
        {
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
            int DtSent = clientSocket.Send(msg, msg.Length, SocketFlags.None);
            if (DtSent == 0)
            {
                MessageBox.Show("Aucune donnée n'a été envoyée");
            }
        }
        private void CheckData()
        {
            try
            {
                while (true)
                {
                    if (this.clientSocket.Connected)
                    {
                        if (this.clientSocket.Poll(10, SelectMode.SelectRead) && this.clientSocket.Available == 0)
                        {
                            MessageBox.Show("La connexion au serveur est interrompue. Essayez avec un autre pseudo");
                            this.LoginButton.Enabled = true;
                            Thread.CurrentThread.Abort();
                        }

                        if (this.clientSocket.Available > 0)
                        {
                            string messageReceived = null;

                            while (this.clientSocket.Available > 0)
                            {
                                try
                                {
                                    byte[] msg = new Byte[clientSocket.Available];
                                    //Réception des données
                                    clientSocket.Receive(msg, 0, clientSocket.Available, SocketFlags.None);
                                    messageReceived = System.Text.Encoding.UTF8.GetString(msg).Trim();
                                    //On concatène les données reues(max 4ko) dans une variable de la classe
                                    contentReceived += messageReceived;
                                }
                                catch (SocketException E)
                                {
                                    MessageBox.Show("CheckData read" + E.Message);
                                }
                            }
                            try
                            {
                                //On remplit le richtextbox avec les données reues 
                                //lorsqu'on a tout réceptionné
                                //chatBody.Rtf = rtfStart + contentReceived;
                                this.BringToFront();
                            }
                            catch (Exception E)
                            {
                                MessageBox.Show(E.Message);
                            }

                        }
                    }
                    //On temporise pendant 10 millisecondes, ceci pour éviter
                    //que le micro processeur s'emballe
                    Thread.Sleep(10);
                }
            }
            catch
            {
                //Ce thread étant susceptible d'tre arrté à tout moment
                //on catch l'exception afin de ne pas afficher un message à l'utilisateur
                Thread.ResetAbort();
            }
        }
    }
}
