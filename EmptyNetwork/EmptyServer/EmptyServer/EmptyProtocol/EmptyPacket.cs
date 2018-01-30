using FlatBuffers;
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
        //protected LightBuffer builder;
        public int seq;
        public E_PROTOCOL_TYPE eProtocolType;

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
            //builder = new ByteBuffer(new byte[32]);
            //builder.PutInt(0, 1);
            //builder.PutInt(0, 1);
            //return builder.DataBuffer.Data;
            return null;
        }

        private int GetBufferObjectCount()
        {
            return 5 + GetFlatBufferObjectCount();
        }

        protected abstract int GetFlatBufferObjectCount();
        protected abstract void AddFlatBufferObjects();
        protected virtual void Deserialize(ByteBuffer byteBuffer)
        {
            int offset = 24;
            seq = byteBuffer.GetInt(offset+0);
            eProtocolType = (E_PROTOCOL_TYPE) byteBuffer.GetInt(offset+1);
        }

        public static ByteBuffer Deserialize(byte[] data)
        {
            return new ByteBuffer(data);
        }
    }
}
