using EmptyProtocol;
using EmptyServer;
using System;
using System.Net;

namespace EmptyClient
{
    public class Program
    {
        public static int Main(String[] args)
        {
            try
            {
                NetworkManager server = new NetworkManager((Dns.GetHostEntry(Dns.GetHostName())).IfNotNull(ipHostInfo => { return ipHostInfo.AddressList[1]; }), 11000);
                Console.ReadLine();
                server.Init();
                server.Connect();
                while (true)
                {
                    Console.ReadLine();
                    server.Send(new DummyPacket());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }
    }
}