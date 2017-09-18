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
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class ChatServer
    {
        // private string hostname { get; set; } = "";
        // private int port { get; set; } = 0;

        // Thread signal handler:  
        public static ManualResetEvent _allDone = new ManualResetEvent(false);

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
            state.workSocket = new_client;
            _clients.Add(new_client);

            new_client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }
        
        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            try
            {
                // Read data from the client socket.   
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.UTF8.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read   
                    // more data.  
                    content = state.sb.ToString();
                    Debug.WriteLine("Message received" + content);
                    //if (content.IndexOf("<EOF>") > -1)
                    //{
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    // Echo the data back to the client.
                    ChatMessage chatMessage = JsonParser.ParseStringToObject<ChatMessage>(content);
                        Send(handler, chatMessage);
                    //}
                    //else
                    //{
                    //    // Not all data received. Get more.  
                    //    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //    new AsyncCallback(ReadCallback), state);
                    //}
                }
            }
            catch(Exception)
            {

            }
            finally
            {
                state.buffer = new byte[1024];
                state.sb = new StringBuilder();
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
        }

        private void Send(Socket handler, ChatMessage data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(JsonParser.ParseObjectToString(data));

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;

                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
                state.buffer = new byte[1024];
                state.sb = new StringBuilder();
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}