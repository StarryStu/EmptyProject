using System;

namespace LightBuffers
{

    public class LightBuffer
    {
        public byte[] buffer;
        public int offset;

        public LightBuffer(byte[] buffer)
        {
            this.buffer = buffer;
            offset = 0;
        }
        
        public LightDataType ReadDataType()
        {
            return (LightDataType)ReadByte();
        }

        public void SkipByte()
        {
            offset++;
        }

        public byte ReadByte()
        {
            byte value = buffer[offset];
            SkipByte();
            return value;
        }

        public void SkipInt()
        {
            offset += sizeof(int);
        }

        public int ReadInt()
        {
            int value = BitConverter.ToInt32(buffer, offset);
            SkipInt();
            return value;
        }

        public void SkipShort()
        {
            offset += sizeof(short);
        }

        public short ReadShort()
        {
            short value = BitConverter.ToInt16(buffer, offset);
            SkipShort();
            return value;
        }
    }
}
