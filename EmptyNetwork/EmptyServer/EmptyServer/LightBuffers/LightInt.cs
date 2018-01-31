using System.Text;

namespace LightBuffers
{
    public struct LightInt : ILightData
    {
        private byte key;
        private int value;

        public int Value { get => value; set => this.value = value; }

        public LightInt(byte key, int value)
        {
            this.key = key;
            this.value = value;
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[GetByteSize()];
            buffer[0] = (int)LightDataType.Int;
            buffer[1] = key;

            for (int i = 0; i < sizeof(int); i++)
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
            return sizeof(byte) + sizeof(byte) + sizeof(int);
        }

        public LightDataType GetDataType()
        {
            return LightDataType.Int;
        }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int parentCount)
        {
            StringBuilder builder = new StringBuilder();
            return string.Format("[{0}] key:{1}, value:{2}", GetDataType().ToString(), key, value);
        }
    }
}
