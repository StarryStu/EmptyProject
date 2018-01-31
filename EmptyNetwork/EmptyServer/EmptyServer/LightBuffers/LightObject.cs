using System;
using System.Collections.Generic;
using System.Text;

namespace LightBuffers
{
    public class LightObject : ILightData
    {
        private Queue<ILightData> dataQueue = new Queue<ILightData>();
        public Dictionary<byte, ILightData> dataDic = new Dictionary<byte, ILightData>();

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

        private void AddLightData(ILightData data)
        {
            byte key = data.GetKey();
            dataQueue.Enqueue(data);

            if (dataDic.ContainsKey(key))
                dataDic[key] = data;
            else
                dataDic.Add(key, data);
        }
        #region Put
        public void PutInt(byte key, int value)
        {
            AddLightData(new LightInt(key, value));
        }

        public void PutObject(byte key, LightObject obj)
        {
            obj.key = key;
            AddLightData(obj);
        }
        #endregion
        #region Get
        public int GetInt(byte key)
        {
            if (!ContainsKey(key))
                return default(int);
            return ((LightInt)dataDic[key]).Value;
        }

        public LightObject GetObject(byte key)
        {
            if (!ContainsKey(key))
                return default(LightObject);
            return ((LightObject)dataDic[key]);
        }
        #endregion

        public bool ContainsKey(byte key)
        {
            return dataDic.ContainsKey(key);
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
            return (LightObject)ParseData(new LightBuffer(buffer));
        }

        public static ILightData ParseData(LightBuffer lightBuffer)
        {
            ILightData data = null;
            LightDataType dataType = lightBuffer.ReadDataType();

            switch (dataType)
            {
                case LightDataType.Object:
                    byte[] tempBuffer = new byte[lightBuffer.buffer.Length - lightBuffer.offset];
                    Buffer.BlockCopy(lightBuffer.buffer, lightBuffer.offset, tempBuffer, 0, tempBuffer.Length);
                    LightBuffer objectBuffer = new LightBuffer(tempBuffer);
                    LightObject tempObj = new LightObject();
                    tempObj.key = objectBuffer.ReadByte();
                    int dataCount = objectBuffer.ReadByte();
                    for (int i = 0; i < dataCount; i++)
                    {
                        tempObj.AddLightData(ParseData(objectBuffer));
                    }
                    data = tempObj;
                    lightBuffer.offset += objectBuffer.offset;
                    break;
                case LightDataType.Int:
                    data = LightInt.Deserialize(lightBuffer);
                    break;
            }
            return data;
        }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int parentCount)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("[{0}] key:{1}, count:{2}", GetDataType().ToString(), key, dataQueue.Count));
            builder.AppendLine();
            int j = 0;
            int count = dataQueue.Count;
            foreach (ILightData data in dataQueue)
            {
                for (int i = 0; i < parentCount+1; i++)
                    builder.Append("\t");
                if(j < count-1)
                    builder.AppendLine(data.ToString(parentCount + 1));
                else
                    builder.Append(data.ToString(parentCount + 1));
                j++;
            }
            return builder.ToString();
        }

        public byte GetKey()
        {
            return key;
        }
    }
}
