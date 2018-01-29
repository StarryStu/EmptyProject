using System;

namespace EmptyProtocol
{
    [Serializable]
    public class DummyPacket : Packet
    {
        public override E_PROTOCOL_TYPE GetProtocolType()
        {
            return E_PROTOCOL_TYPE.Dummy;
        }
    }
}
