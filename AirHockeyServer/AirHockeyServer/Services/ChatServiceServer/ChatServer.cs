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
        private string hostname { get; set; } = "";

        private int port { get; set; } = 0;

        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private List<Socket> _clients = new List<Socket>();

        public ChatServer()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            Socket.Bind(new IPEndPoint(IPAddress.Any, 8080));
            Socket.Listen(128);
        }

        Socket Socket { get; set; }
        public void StartListeningAsync()
        {
            Socket.BeginAccept(null, 0, new AsyncCallback(OnAccept), null);
        }

        private void OnAccept(IAsyncResult result)
        {
            Debug.WriteLine("Server accepted a connection");
            // Get the socket that handles the client request.  
            //Socket listener = (Socket)result.AsyncState;
            Socket handler = Socket.EndAccept(result);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;

            _clients.Add(handler);

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
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