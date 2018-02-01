using EmptyEngine;
using EmptyProtocol;
using FlatBuffers;
using LightBuffers;

namespace EmptyClient
{
    public class DummyProcess : BaseProcess
    {
        public override void Process(LightObject lightObj)
        {
            DummyPacket packet = new DummyPacket(lightObj);
            Debugs.Log(lightObj);
        }
    }
}
