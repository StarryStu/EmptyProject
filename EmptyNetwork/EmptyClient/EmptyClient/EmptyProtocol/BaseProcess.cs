using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace EmptyProtocol
{
    public abstract class BaseProcess
    {
        public virtual void StartProcess(byte[] data)
        {
            Packet packet = Deserialize(data);
            Process(packet);
        }

        public abstract void Process(Packet packet);

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
