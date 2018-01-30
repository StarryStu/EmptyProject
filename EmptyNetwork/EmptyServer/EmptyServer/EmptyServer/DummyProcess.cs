using EmptyEngine;
using EmptyProtocol;
using FlatBuffers;

namespace EmptyServer
{
    public class DummyProcess : BaseProcess
    {
        public override void Process(ByteBuffer packet)
        {
            DummyPacket data = new DummyPacket(packet);

            Debugs.Log(data);
        }
    }
}
