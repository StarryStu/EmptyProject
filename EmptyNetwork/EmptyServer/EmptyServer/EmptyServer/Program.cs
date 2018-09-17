using EmptyProtocol;
using System;
using System.Net;

namespace EmptyServer
{
    public class Program
    {
        public static int Main(String[] args)
        {
            try
            {
                NetworkManager server = new NetworkManager((Dns.GetHostEntry(Dns.GetHostName())).IfNotNull(ipHostInfo => { return ipHostInfo.AddressList[1]; }), 11000);

                server.packetHandler.AddProtocol(new Protocol(E_PROTOCOL_TYPE.Dummy, new DummyProcess()));

                server.Init();
                server.Listen();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }
    }
}