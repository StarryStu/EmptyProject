using FlatBuffers;
using LightBuffers;
using System;

namespace EmptyProtocol
{
    [Serializable]
    public class DummyPacket : Packet
    {
        public DummyPacket() : base()
        {

        }

        public DummyPacket(LightObject receiveObject) : base()
        { 
            Deserialize(receiveObject);
        }

        public override E_PROTOCOL_TYPE GetProtocolType()
        {
            return E_PROTOCOL_TYPE.Dummy;
        }

        protected override void WriteLightObject()
        {

        }

        protected override void ReadLightObject()
        {

        }
    }
}
