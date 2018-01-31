using EmptyEngine;
using EmptyProtocol;
using LightBuffers;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace EmptyServer
{
    public class EmptyServerMain
    {
        private ServerProtocolEvent serverProtocolEvent = new ServerProtocolEvent();

        private Socket listener;
        private int port;
        private IPAddress ipAddress;

        static byte[] savedData;
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public EmptyServerMain(IPAddress ipAddress, int port)
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

        public void Start()
        {
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                Update();
            }
        }

        private void Update()
        {
            // Set the event to nonsignaled state.
            allDone.Reset();

            // Start an asynchronous socket to listen for connections.
            Console.WriteLine("Waiting for a connection...");
            listener.BeginAccept(
                new AsyncCallback(AcceptCallback),
                listener);

            // Wait until a connection is made before continuing.
            allDone.WaitOne();
        }


        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

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
                serverProtocolEvent.Receieve(data);

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

        private void Send(Socket handler, Packet packet)
        {
            Send(handler, packet.Serialize());
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

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static int Main(String[] args)
        {
            try
            {
                EmptyServerMain server = new EmptyServerMain((Dns.GetHostEntry(Dns.GetHostName())).IfNotNull(ipHostInfo => { return ipHostInfo.AddressList[1]; }), 11000);
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }
    }
}
