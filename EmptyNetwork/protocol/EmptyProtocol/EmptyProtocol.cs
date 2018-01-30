namespace EmptyProtocol
{
    public enum E_PROTOCOL_TYPE
    {
        Dummy = -1,
        Login = 0,
    }

    public class Protocol
    {
        public E_PROTOCOL_TYPE type;
        public BaseProcess process;

        public Protocol(E_PROTOCOL_TYPE type, BaseProcess process)
        {
            this.type = type;
            this.process = process;
        }

        public void Process(Packet packet)
        {
            process.Process(packet);
        }
    }

}
