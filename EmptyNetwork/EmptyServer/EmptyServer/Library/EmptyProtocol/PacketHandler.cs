using EmptyEngine;
using EmptyServer;
using FlatBuffers;
using LightBuffers;
using System;
using System.Collections.Generic;

namespace EmptyProtocol
{
    public class PacketHandler
    {
        private Dictionary<E_PROTOCOL_TYPE, Protocol> protocolList = new Dictionary<E_PROTOCOL_TYPE, Protocol>();

        public PacketHandler()
        {
            //Init();
        }

        private void Init()
        {
            //AddProtocol(new Protocol(E_PROTOCOL_TYPE.Dummy, new DummyProcess()));
            //AddProtocol(new Protocol(E_PROTOCOL_TYPE.Login, new LoginProcess()));
        }

        public void AddProtocol(Protocol protocol)
        {
            if (protocolList.ContainsKey(protocol.type))
            {
                Debugs.Log("[EmptyProtocol] 중복된 프로토콜 등록이있습니다!");
                return;
            }
            protocolList.Add(protocol.type, protocol);
        }

        public void Receieve(byte[] data)
        {
            FindProtocol(data);
        }

        private void FindProtocol(byte[] data)
        {
            LightObject lightObj = LightObject.Deserialize(data);
            lightObj.PrintDump();
            E_PROTOCOL_TYPE protocolType = (E_PROTOCOL_TYPE)lightObj.GetInt(1);
            Protocol targetProtocol = FindProtocol(protocolType);
            if (targetProtocol == null)
            {
                Debugs.Log("[EmptyProtocol] 해당 프로토콜을 찾을 수 없습니다.");
                return;
            }
            targetProtocol.Process(lightObj);
        }

        private Protocol FindProtocol(E_PROTOCOL_TYPE eProtocolType)
        {
            if (!(protocolList.ContainsKey(eProtocolType)))
                return null;
            return protocolList[eProtocolType];
        }
    }
}
