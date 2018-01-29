using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace EmptyProtocol
{
    [Serializable]
    public abstract class Packet
    {
        protected int seq;
        protected E_PROTOCOL_TYPE eProtocolType;

        public Packet()
        {
            SetProtocolType();
        }

        private void SetProtocolType()
        {
            eProtocolType = GetProtocolType();
        }

        public abstract E_PROTOCOL_TYPE GetProtocolType();

        public override string ToString()
        {
            return string.Format("[Packet]\n[seq : {0}]\n[protocol : {1}]", seq.ToString(), eProtocolType.ToString());
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter br = new BinaryFormatter();
                br.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        public static Packet Deserialize(byte[] data)
        {
            Packet packet = null;
            using (MemoryStream ms = new MemoryStream(data))
            {
                IFormatter br = new BinaryFormatter();
                packet = (br.Deserialize(ms) as Packet);
            }
            return packet;
        }
    }
}
