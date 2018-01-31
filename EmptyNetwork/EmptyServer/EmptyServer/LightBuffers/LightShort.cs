using System.Text;

namespace LightBuffers
{
    public struct LightShort : ILightData
    {
        private byte key;
        private short value;

        public short Value { get => value; set => this.value = value; }

        public LightShort(byte key, short value)
        {
            this.key = key;
            this.value = value;
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[GetByteSize()];
            buffer[0] = (byte)LightDataType.Short;
            buffer[1] = key;

            for (int i = 0; i < sizeof(short); i++)
            {
                buffer[2 + i] = (byte)(value >> i * 8);
            }
            return buffer;
        }

        public static LightInt Deserialize(LightBuffer buffer)
        {
            return new LightInt(buffer.ReadByte(), buffer.ReadInt());
        }

        public byte GetKey()
        {
            return key;
        }

        public int GetByteSize()
        {
            return sizeof(byte) + sizeof(byte) + sizeof(short);
        }

        public LightDataType GetDataType()
        {
            return LightDataType.Short;
        }

        public string ToString(int parentCount)
        {
            StringBuilder builder = new StringBuilder();
            return string.Format("[{0}] key:{1}, value:{2}", GetDataType().ToString(), key, value);
        }

    }
}
