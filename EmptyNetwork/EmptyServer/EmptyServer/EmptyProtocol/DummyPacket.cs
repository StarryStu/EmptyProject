using FlatBuffers;
using System;

namespace EmptyProtocol
{
    [Serializable]
    public class DummyPacket : Packet
    {
        public DummyPacket() : base()
        {

        }

        public DummyPacket(ByteBuffer buffer) : base()
        { 
            Deserialize(buffer);
        }

        public override E_PROTOCOL_TYPE GetProtocolType()
        {
            return E_PROTOCOL_TYPE.Dummy;
        }

        protected override void AddFlatBufferObjects()
        {

        }

        protected override int GetFlatBufferObjectCount()
        {
            return 0;
        }
    }
}
