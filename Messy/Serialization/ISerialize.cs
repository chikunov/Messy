using System.IO;

namespace Messy.Serialization
{
    public interface ISerialize
    {
        T Deserialize<T>(Stream input);
        void Serialize<T>(Stream output, T graph);
    }
}