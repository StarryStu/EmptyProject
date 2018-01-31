using FlatBuffers;
using LightBuffers;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace EmptyProtocol
{
    public abstract class BaseProcess
    {
        public abstract void Process(LightObject lgihtObj);
    }
}
