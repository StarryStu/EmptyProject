using EmptyEngine;
using EmptyProtocol;

namespace EmptyServer
{
    public class DummyProcess : BaseProcess
    {
        public override void Process(Packet packet)
        {
            DummyPacket data = packet as DummyPacket;
            if (data == null)
                return;

            Debugs.Log(data);
        }
    }
}
