using FlatBuffers;
using LightBuffers;
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

        public LightObject Serialize()
        {
            LightObject sendObj = new LightObject();
            sendObj.PutInt(0, seq);
            sendObj.PutInt(1, (int)eProtocolType);
            WriteLightObject();
            return sendObj;
        }
        protected abstract void WriteLightObject();
        protected abstract void ReadLightObject();

        protected virtual void Deserialize(LightObject receiveObject)
        {
            seq = receiveObject.GetInt(0);
            eProtocolType = (E_PROTOCOL_TYPE)receiveObject.GetInt(1);
            ReadLightObject();
        }
    }
}
