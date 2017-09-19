using AirHockeyServer.Entities;
using AirHockeyServer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace AirHockeyServer.Services.ChatServiceServer
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        public Socket ClientSocket = null;
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder Message = new StringBuilder();
    }

    public class ChatServer
    {
        // private string hostname { get; set; } = "";
        // private int port { get; set; } = 0;

        // Thread signal handler:  
        private static ManualResetEvent _allDone = new ManualResetEvent(false);

        // We define a socket for listening on an endpoint and
        // accept (future) connections:
        private Socket _listener { get; set; }

        private List<Socket> _clients = new List<Socket>();

        public ChatServer()
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _listener.Bind(new IPEndPoint(IPAddress.Any, 8080));
            _listener.Listen(128); // Max of 128 (simultaneous) connections.
        }

        public void StartListeningAsync()
        {
            while (true)
            {
                _allDone.Reset();

                Debug.WriteLine("Waiting for a connection...");

                _listener.BeginAccept(new AsyncCallback(OnAccept), _listener);

                // Blocking wait for waiting a new connection:
                _allDone.WaitOne();
            }
        }

        private void OnAccept(IAsyncResult result)
        {
            // Notify the calling thread (while loop) that a new
            // connection has been made and that we can continue to
            // wait for other connections:
            _allDone.Set();

            Debug.WriteLine("Server accepted a connection");

            // We retrieve our _listener through result and we don't use
            // directly _listener because we are in threads and the memory is
            // shared (so using directly _listener could lead to undefined behavior
            // while retrieving _listener from result is thread-safe):
            Socket listener = (Socket) result.AsyncState;

            Socket new_client = listener.EndAccept(result);
            StateObject state = new StateObject();
            state.ClientSocket = new_client;
            _clients.Add(new_client);

            new_client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }
        
        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                // Retrieve the state object and the client socket  
                // from the asynchronous state object:  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.ClientSocket;

                // Read data from the client socket.   
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far:
                    state.Message.Append(Encoding.UTF8.GetString(
                        state.Buffer, 0, bytesRead));
                    content = state.Message.ToString();

                    Debug.WriteLine("Message received" + content);

                    ChatMessage chatMessage = JsonParser.ParseStringToObject<ChatMessage>(content);
                    Send(client, chatMessage);
                }

                // We continue to asynchronously read what the client is sending
                // to us:
                state.Message.Clear();
                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private void Send(Socket client, ChatMessage message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonParser.ParseObjectToString(message));
            client.BeginSend(bytes, 0, bytes.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket) ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                Debug.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}