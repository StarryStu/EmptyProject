namespace LightBuffers
{
    public enum LightDataType
    {
        Object,
        Int,
        Short,
    }

    public interface ILightData
    {
        byte[] Serialize();
        byte GetKey();
        int GetByteSize();
        LightDataType GetDataType();
        string ToString(int parentCount);
    }
}
