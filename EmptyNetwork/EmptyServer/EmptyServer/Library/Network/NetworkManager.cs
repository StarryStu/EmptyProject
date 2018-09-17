using EmptyEngine;
using EmptyProtocol;
using LightBuffers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace EmptyServer
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
    }

    public class NetworkManager
    {
        public PacketHandler packetHandler = new PacketHandler();

        private Socket socket;
        private int port;
        private IPAddress ipAddress;

        private IPEndPoint endPoint;

        // Thread signal.
        public ManualResetEvent signal = new ManualResetEvent(false);

        public NetworkManager(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void TestStart()
        {
            try
            {
                LightObject lightObject = new LightObject();

                LightObject dummy = new LightObject();
                dummy.PutInt(1, 1000);
                dummy.PutInt(2, 10);

                LightObject dummy2 = new LightObject();
                dummy2.PutInt(1, 1000);
                dummy2.PutInt(2, 10);

                dummy.PutObject(1, dummy2);

                lightObject.PutObject(2, dummy);
                lightObject.PutObject(3, dummy2);

                byte[] data = lightObject.Serialize();

                LightObject coveredObject = LightObject.Deserialize(data);

                coveredObject.GetInt(1);

                Debugs.Log(coveredObject);
            }
            catch(Exception e)
            {
                Debugs.Log(e);
            }
        }

        public void Init()
        {
            Debugs.Log("[NetworkManager] Init");
            endPoint = new IPEndPoint(ipAddress, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Listen()
        {
            Debugs.Log("[NetworkManager] Listen");
            socket.Bind(endPoint);
            socket.Listen(100);

            while (true)
            {
                BeginAccept();
            }
        }

        public void Connect()
        {
            socket.BeginConnect(endPoint,
                new AsyncCallback(ConnectCallBack), socket);
            signal.WaitOne();
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket socket = (Socket)ar.AsyncState;

                // Complete the connection.
                socket.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    socket.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                signal.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void BeginAccept()
        {
            Debugs.Log("[NetworkManager] BeginAccept");
            // Set the event to nonsignaled state.
            signal.Reset();

            socket.BeginAccept(
                new AsyncCallback(AcceptCallback),
                socket);

            // Wait until a connection is made before continuing.
            signal.WaitOne();
        }


        public void AcceptCallback(IAsyncResult ar)
        {
            Debugs.Log("[NetworkManager] AcceptCallback");
            // Signal the main thread to continue.
            signal.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            Debugs.Log("[NetworkManager] ReadCallback");
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                byte[] data = new byte[bytesRead];
                System.Buffer.BlockCopy(state.buffer, 0, data, 0, bytesRead);
                packetHandler.Receieve(data);

                //// There  might be more data, so store the data received so far.
                //state.sb.Append(Encoding.ASCII.GetString(
                //    state.buffer, 0, bytesRead));

                //// Check for end-of-file tag. If it is not there, read 
                //// more data.
                //content = state.sb.ToString();
                //if (content.IndexOf("<EOF>") > -1)
                //{
                //    // All the data has been read from the 
                //    // client. Display it on the console.
                //    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                //        content.Length, content);
                //    // Echo the data back to the client.
                //    Send(handler, content);
                //}
                //else
                //{
                //    // Not all data received. Get more.
                //    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                //    new AsyncCallback(ReadCallback), state);
                //}
            }
        }

        public void Send(Packet packet)
        {
            Send(socket, packet.Serialize());
        }

        private void Send(Socket handler, LightObject data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = data.Serialize();

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
