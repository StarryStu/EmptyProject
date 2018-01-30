//using System;

//namespace EmptyProtocol
//{
//    [Serializable]
//    public class LoginPacket : Packet
//    {
//        protected int id;
//        protected int password;

//        public override E_PROTOCOL_TYPE GetProtocolType()
//        {
//            return E_PROTOCOL_TYPE.Login;
//        }

//        public override string ToString()
//        {
//            return string.Concat(base.ToString(), "\n",
//                 string.Format("[LoginPacket]\n[id : {0}]\n[password : {1}]", id.ToString(), password.ToString())
//                );
//        }
//    }
//}
