using System;
using System.Collections;
using System.Collections.Generic;

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
    }
    public class LightObject : ILightData
    {
        private Queue<ILightData> dataQueue = new Queue<ILightData>();
        public Dictionary<byte,ILightData> dataDic = new Dictionary<byte, ILightData>();

        public byte key;

        public LightObject()
        {
            
        }

        public int GetByteSize()
        {
            int total = 0;
            foreach (ILightData data in dataQueue)
            {
                total += data.GetByteSize();
            }
            return 3 + total;
        }

        public LightDataType GetDataType()
        {
            return LightDataType.Object;
        }

        private void AddLightData(byte key, ILightData data)
        {
            dataQueue.Enqueue(data);

            if (dataDic.ContainsKey(key))
                dataDic[key] = data;
            else
                dataDic.Add(key, data);
        }

        public void PutInt(byte key, int value)
        {
            AddLightData(key, new LightInt(key, value));
        }

        public void PutObject(byte key, LightObject obj)
        {
            AddLightData(key, obj);
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[GetByteSize()];

            buffer[0] = (byte)GetDataType();
            buffer[1] = key;
            buffer[2] = (byte)dataQueue.Count;

            int offset = 3;
            foreach (ILightData data in dataQueue)
            {
                Buffer.BlockCopy(data.Serialize(), 0, buffer, offset, data.GetByteSize());
                offset += data.GetByteSize();
            }
            return buffer;
        }

        public static LightObject Deserialize(byte[] buffer)
        {
            LightBuffer lightBuffer = new LightBuffer(buffer);
            LightObject tempObj = new LightObject();
            LightDataType dataType = (LightDataType)lightBuffer.ReadByte();
            tempObj.key = lightBuffer.ReadByte();
            int dataCount = lightBuffer.ReadByte();

            for (int i = 0; i < dataCount; i++)
            {
                tempObj.dataQueue.Enqueue(ParseData(lightBuffer));
            }

            return tempObj;
        }

        public static ILightData ParseData(LightBuffer buffer)
        {
            ILightData data = null;
            LightDataType dataType = buffer.ReadDataType();

            switch (dataType)
            {
                case LightDataType.Object:
                    byte[] tempBuffer = new byte[buffer.buffer.Length - buffer.offset];
                    Buffer.BlockCopy(buffer.buffer, buffer.offset, tempBuffer, 0, tempBuffer.Length);
                    LightBuffer lightBuffer = new LightBuffer(tempBuffer);
                    LightObject tempObj = new LightObject();
                    tempObj.key = lightBuffer.ReadByte();
                    int dataCount = lightBuffer.ReadByte();

                    for (int i = 0; i < dataCount; i++)
                    {
                        tempObj.dataQueue.Enqueue(ParseData(lightBuffer));
                    }
                    data = tempObj;
                    break;
                case LightDataType.Int:
                    data = LightInt.Deserialize(buffer);
                    break;
            }
            return data;
        }
        
    }

    public struct LightInt : ILightData
    {
        private byte key;
        private int value;

        public LightInt(byte key, int value)
        {
            this.key = key;
            this.value = value;
        }

        public int GetByteSize()
        {
            return 6;
        }

        public LightDataType GetDataType()
        {
            return LightDataType.Int;
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
    }

    public interface ILightData
    {
        byte[] Serialize();
        LightDataType GetDataType();
        int GetByteSize();
    }

    public enum LightDataType
    {
        Object,
        Int,
    }
}
